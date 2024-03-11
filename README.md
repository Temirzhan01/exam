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
	"time"
)

type Row struct {
	Refer  string    `gorm:"default:unique;type:uuid;primary_key" json:"refer"`
	Status bool      `json:"status"` // is Ready status
	IinBIN string    `json:"iinBIN"`
	Checks []Check   `json:"checks"`
	Date   time.Time `json:"date"`
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
