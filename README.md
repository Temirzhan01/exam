
Cannot use 'repository.CompraRepoitory{repository.NewDb(config)}' (type repository.CompraRepoitory) as the type repository.IRepository Type does not implement 'repository.IRepository' as the 'SaveRow' method has a pointer receiver

package repository

import (
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
)

type IRepository interface {
	SaveRow(row model.Row)
	GetRow(row model.Row)
}


package repository

import (
	"fmt"
	"time"

	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/postgres"
	"halykbpm-git.homebank.kz/business-processes/compraIP/config"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
)

type CompraRepoitory struct {
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
	db.AutoMigrate()
	return db
}

func (dbm *CompraRepoitory) SaveRow(model.Row) {

}

func (dbm *CompraRepoitory) GetRow(model.Row) {

}
