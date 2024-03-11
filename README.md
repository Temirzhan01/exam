(C:/Users/00055864/go/src/halykbpm-git.homebank.kz/business-processes/compra.integration/repository/compraRepository.go:60) 
[2024-03-11 17:43:43]  [3.25ms]  SELECT count(*) FROM ""  WHERE (refer = '8992bc27-df59-4fdc-8850-c2f89912bef7')
[0 rows affected or returned ]

(C:/Users/00055864/go/src/halykbpm-git.homebank.kz/business-processes/compra.integration/repository/compraRepository.go:60)
[2024-03-11 17:43:43]  pq: zero-length delimited identifier at or near """" 

(C:/Users/00055864/go/src/halykbpm-git.homebank.kz/business-processes/compra.integration/repository/compraRepository.go:47)
[2024-03-11 17:43:43]  pq: invalid input syntax for type json 

(C:/Users/00055864/go/src/halykbpm-git.homebank.kz/business-processes/compra.integration/repository/compraRepository.go:47)
[2024-03-11 17:43:43]  [4.80ms]  UPDATE "compraCheckResults" SET "status" = false, "iin_bin" = '010904500574', "checks" = '[]', "date" = '2024-03-11 17:43:43'  WHERE "compraCheckResults"."refer" = '8992bc27-df59-4fdc-8850-c2f899
12bef7'
[0 rows affected or returned ]
pq: invalid input syntax for type json
&{8992bc27-df59-4fdc-8850-c2f89912bef7 false 010904500574 [] 2024-03-11 17:43:43.4805005 +0500 +05 m=+7.978002501}


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
	fmt.Println(err)
	fmt.Println(row)
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
