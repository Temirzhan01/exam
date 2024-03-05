package app

import (
	"fmt"
	"net"
	"net/http"
	"time"

	"halykbpm-git.homebank.kz/business-processes/compra.integraion/config"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/handler"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/repository"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/router"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/service"
	"halykbpm-git.homebank.kz/business-processes/compra.integraion/util/logger"
)

//Start ...
func Start(config *config.Config) {
	log := logger.NewLogger(config.ESURL, config.LogLevel)

	tout := time.Duration(config.ServiceTimeoutSeconds) * time.Second
	var client *http.Client
	var defaultTransport http.RoundTripper = &http.Transport{
		Proxy: http.ProxyFromEnvironment,
		DialContext: (&net.Dialer{
			Timeout:   10 * time.Second,
			KeepAlive: 10 * time.Second,
			DualStack: true,
		}).DialContext,
		ForceAttemptHTTP2:     true,
		MaxIdleConns:          200,
		MaxIdleConnsPerHost:   100,
		IdleConnTimeout:       30 * time.Second,
		TLSHandshakeTimeout:   10 * time.Second,
		ExpectContinueTimeout: 1 * time.Second,
		MaxConnsPerHost:       30,
	}
	if config.LogServiceRequests && !config.ReleaseMode {
		client = &http.Client{
			Transport: logger.NewLoggedRoundTripper(http.DefaultTransport, logger.NewDefaultLogger()),
			Timeout:   tout,
		}
	} else {
		client = &http.Client{
			Transport: defaultTransport,
			Timeout:   tout,
		}
	}

	h := handler.NewHandler(&repository.CompraRepository{repository.NewDb(config)}, &service.CompraService{Integraion: service.NewIntegrationService(log, config, service.WithHTTPClient(client))}, config, log)
	r := router.InitRouter(h, config.ReleaseMode)

	log.Info("compra.integraion", "START", "Starting", "", "", "", "", nil)
	if err := r.Run(fmt.Sprintf(":%d", config.ServerPort)); err != nil {
		log.Fatal("compra.integraion", "START", -1, "Failed start server", "", "", "", "", nil)
	}
}
