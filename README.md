package router

import (
	"github.com/gin-gonic/gin"
	ginSwagger "github.com/swaggo/gin-swagger"
	"github.com/swaggo/gin-swagger/swaggerFiles"
	_ "halykbpm-git.homebank.kz/business-processes/compra.integration/docs"
	"halykbpm-git.homebank.kz/business-processes/compra.integration/handler"
	"io/ioutil"
)

// InitRouter initialize routing information
func InitRouter(h *handler.Handler, production bool) *gin.Engine {
	if production {
		gin.SetMode(gin.ReleaseMode)
		gin.DefaultWriter = ioutil.Discard
	}
	r := gin.Default()
	if !production {
		r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	}
	r.POST("/checkIP/iinBIN/:iinBIN", h.CheckIP)
	r.POST("/getCheckResults/refer/:refer", h.GetCheckResults)
	r.GET("/healthz")

	return r
}
