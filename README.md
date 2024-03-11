package repository

import (
	"fmt"
	"github.com/pkg/errors"
	"time"

	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/postgres"
	"halykbpm-git.homebank.kz/business-processes/compra.integration/config"
	"halykbpm-git.homebank.kz/business-processes/compra.integration/model"
)

type CompraRepository struct {
	DB *gorm.DB
}

func NewDb(config *config.Config) *gorm.DB {
	fmt.Println("config.DbHost " + config.DbHost)
	connect := fmt.Sprintf("host=%v port=%v user=%v password=%v dbname=%v sslmode=disable", config.DbHost, config.DbPort, config.DbUser, config.DbPass, config.DbName)
	db, err := gorm.Open("postgres", connect)
	if err != nil {
		panic(err)
	}

	db.LogMode(config.DbLogMode)
	db.DB().SetMaxOpenConns(config.DbMaxOpenConns)
	db.DB().SetMaxIdleConns(config.DbMaxIdleConns)
	db.DB().SetConnMaxLifetime(time.Second * time.Duration(config.DbConnMaxLifetime))
	db.AutoMigrate(
		&model.Row{},
	)
	return db
}

func (repo *CompraRepository) SaveRow(row *model.Row) (err error) {
	if repo.IsExist(row.Refer) {
		var existingRow model.Row
		err = repo.DB.Where("refer = ?", row.Refer).First(&existingRow).Error
		if err != nil {
			return errors.Wrapf(err, "UpdateRow")
		}
		existingRow = *row
		err = repo.DB.Save(existingRow).Error
		return
	}
	err = repo.DB.Save(row).Error
	return
}

func (repo *CompraRepository) GetRow(refer string) (row model.Row, err error) {
	err = repo.DB.Where("refer = ?", refer).First(&row).Error
	return
}

func (repo *CompraRepository) IsExist(refer string) bool {
	var count int
	repo.DB.Where("refer = ?", refer).Count(&count)
	if count > 0 {
		return true
	} else {
		return false
	}
}
package model

import (
	"encoding/json"
	"time"
)

type Row struct {
	Refer  string          `gorm:"type:uuid;primary_key" json:"refer"`
	Status bool            `json:"status"` // is Ready status
	IinBIN string          `json:"iinBIN"`
	Checks json.RawMessage `json:"checks"`
	Date   time.Time       `json:"date"`
}

func (Row) TableName() string {
	return "compraCheckResults"
}

type CompraResponse struct {
	IinBIN  string  `json:"iinBIN"`
	IsReady bool    `json:"isReady"` // is Ready status
	Checks  []Check `json:"checks"`
}

type Check struct {
	CheckName         string `json:"checkName"`
	Status            string `json:"status"` //checkResult
	ApplicationStatus string `json:"applicationStatus"`
}

type Client struct {
	IinBIN string `uri:"iinBIN" binding:"required"`
}

type Refer struct {
	Refer string `uri:"refer" binding:"required"`
}
package service

import (
	"encoding/json"
	"github.com/pkg/errors"
	"halykbpm-git.homebank.kz/business-processes/compra.integration/model"
	"strconv"
	"time"
)

func ParseResults(response *model.RiskFactorsResponse, guid string, iin string, isFirst bool) (*model.Row, error) {
	action := "ParseResults"
	row := model.Row{Refer: guid, Status: false, IinBIN: iin, Date: time.Now()}
	if isFirst {
		return &row, nil
	}

	filtered := FilterResults(response)
	var checksJson []model.Check
	var err error

	if IsReady(filtered) {
		for _, rf := range filtered.RiskFactors {
			res := model.Check{CheckName: "RF_" + strconv.FormatInt(int64(rf.Type.Id), 10), Status: "finished"}
			if rf.Status == "YES" {
				res.ApplicationStatus = "true"
			} else if rf.Status == "NO" {
				res.ApplicationStatus = "false"
			}
			checksJson = append(checksJson, res)
		}
		row.Checks, err = json.Marshal(checksJson)
		if err != nil {
			return nil, errors.Wrapf(err, action)
		}
		row.Status = true
	}
	return &row, nil
}

func FilterResults(response *model.RiskFactorsResponse) *model.RiskFactorsResponse {
	var filtered *model.RiskFactorsResponse
	necessaryFactorsInd := []int{1, 2, 3, 4, 5, 6, 11, 13, 14, 16, 28, 32, 33, 34, 35, 36, 37, 38, 46, 47}
	for _, item := range response.RiskFactors {
		if Contains(necessaryFactorsInd, item.Type.Id) {
			filtered.RiskFactors = append(filtered.RiskFactors, item)
		}
	}
	return filtered
}

func Contains(list []int, elem int) bool {
	for _, item := range list {
		if item == elem {
			return true
		}
	}
	return false
}

func IsReady(response *model.RiskFactorsResponse) bool {
	for _, rf := range response.RiskFactors {
		if rf.Status == "INIT" {
			return false
		}
	}
	return true
}

func ConvertRow(row *model.Row) (*model.CompraResponse, error) {
	action := "ConvertRow"
	var checks []model.Check
	err := json.Unmarshal(row.Checks, &checks)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}
	return &model.CompraResponse{
		IinBIN:  row.IinBIN,
		IsReady: true,
		Checks:  checks,
	}, nil
}
package service

import (
	"fmt"
	"github.com/google/uuid"
	"github.com/pkg/errors"
	"halykbpm-git.homebank.kz/business-processes/compra.integration/model"
	"halykbpm-git.homebank.kz/business-processes/compra.integration/repository"
)

type CompraService struct {
	Integraion Integration
	Redis      RedisService
	Repository repository.IRepository
}

func (s *CompraService) CheckIP(client *model.Client) (string, error) {
	action := "CheckIP"

	actual, isExist, err := s.Redis.isActualExist(client.IinBIN, "")
	if err != nil {
		return "", errors.Wrapf(err, action)
	}
	if isExist {
		return actual.Refer, nil
	}

	refer := uuid.New().String()
	row, err := ParseResults(nil, refer, client.IinBIN, true)
	fmt.Println(row)
	err = s.Repository.SaveRow(row)
	if err != nil {
		return "", errors.Wrapf(err, action)
	}

	err = s.Integraion.FirstReqRiskFactors(client.IinBIN)
	if err != nil {
		return "", errors.Wrapf(err, action)
	}

	return refer, nil
}

func (s *CompraService) GetResults(refer *model.Refer) (*model.CompraResponse, error) {
	action := "GetResults"

	actual, isExist, err := s.Redis.isActualExist("", refer.Refer)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	if isExist {
		err = s.Repository.SaveRow(actual)
		if err != nil {
			return nil, errors.Wrapf(err, action)
		}

		result, err := ConvertRow(actual)
		if err != nil {
			return nil, errors.Wrapf(err, action)
		}

		return result, nil
	}

	exist, err := s.Repository.GetRow(refer.Refer)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	results, err := s.Integraion.GetRiskFactors(exist.IinBIN)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	row, err := ParseResults(results, refer.Refer, exist.Refer, false)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	if row.Status {
		err = s.Redis.SaveRow(row)
		if err != nil {
			return nil, errors.Wrapf(err, action)
		}

		err = s.Repository.SaveRow(row)
		if err != nil {
			return nil, errors.Wrapf(err, action)
		}
	}

	result, err := ConvertRow(row)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	return result, nil
}
