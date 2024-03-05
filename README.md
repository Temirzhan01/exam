runtime error: invalid memory address or nil pointer dereference
C:/Program Files/Go/src/runtime/panic.go:221 (0x309e1c)
        panicmem: panic(memoryError)
C:/Program Files/Go/src/runtime/signal_windows.go:254 (0x309dec)
        sigpanic: panicmem()
C:/Users/00055864/go/src/halykbpm-git.homebank.kz/business-processes/compra.integration/service/compraService.go:28 (0x91d411)
        (*CompraService).CheckIP: err = s.Repository.SaveRow(row)


package service

import (
	"github.com/google/uuid"
	"github.com/pkg/errors"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/model"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/repository"
)

type CompraService struct {
	Integraion Integration
	Repository repository.IRepository
}

func (s *CompraService) CheckIP(req *model.Client) (string, error) {
	action := "CheckIP"
	guid := uuid.New().String()
	results, err := s.Integraion.GetRiskFactors(req.IinBIN, true)
	if err != nil {
		return "", errors.Wrapf(err, action)
	}

	row, err := ParseResults(results, guid, req.IinBIN, true)
	if err != nil {
		return "", errors.Wrapf(err, action)
	}

	err = s.Repository.SaveRow(row)
	if err != nil {
		return "", errors.Wrapf(err, action)
	}

	return row.Refer, nil
}

func (s *CompraService) GetResults(refer *model.Refer) (*model.Row, error) {
	action := "GetResults"
	exist, err := s.Repository.GetRow(refer.Refer)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	results, err := s.Integraion.GetRiskFactors(exist.Refer, true)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	row, err := ParseResults(results, refer.Refer, exist.Refer, true)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	return row, nil
}
package repository

import (
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/model"
)

type IRepository interface {
	SaveRow(row *model.Row) (err error)
	UpdateRow(row *model.Row) (err error)
	GetRow(refer string) (row model.Row, err error)
	IsExist(refer string) bool
}
package repository

import (
	"fmt"
	"github.com/pkg/errors"
	"time"

	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/postgres"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/config"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/model"
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
	err = repo.DB.Save(row).Error
	return
}

func (repo *CompraRepository) UpdateRow(row *model.Row) (err error) {
	var existingRow model.Row
	err = repo.DB.Where("refer = ?", row.Refer).First(&existingRow).Error
	if err != nil {
		return errors.Wrapf(err, "UpdateRow")
	}
	existingRow = *row
	err = repo.DB.Save(existingRow).Error
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
