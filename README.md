package main

import (
	"fmt"
	"halykbpm-git.homebank.kz/business-processes/compraIP/app"
	"halykbpm-git.homebank.kz/business-processes/compraIP/config"
	"halykbpm-git.homebank.kz/business-processes/compraIP/docs"
	"os"
)

func main() {
	config := config.New("config.json")
	if os.Getenv("APP_MODE") == "development" {
		docs.SwaggerInfo.Host = fmt.Sprintf("localhost:%d", config.ServerPort)
	}
	app.Start(config)
}

package app

import (
	"fmt"
	"net"
	"net/http"
	"time"

	"halykbpm-git.homebank.kz/business-processes/compraIP/config"
	"halykbpm-git.homebank.kz/business-processes/compraIP/handler"
	"halykbpm-git.homebank.kz/business-processes/compraIP/repository/taxes"
	"halykbpm-git.homebank.kz/business-processes/compraIP/router"
	"halykbpm-git.homebank.kz/business-processes/compraIP/service"
	"halykbpm-git.homebank.kz/business-processes/compraIP/util/logger"
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

	store := taxes.New(service.NewCompraService(log, config, service.WithHTTPClient(client)))

	h := handler.NewHandler(store, config, log)
	r := router.InitRouter(h, config.ReleaseMode)

	log.Info("compraIP", "START", "Starting", "", "", "", "", nil)
	if err := r.Run(fmt.Sprintf(":%d", config.ServerPort)); err != nil {
		log.Fatal("compraIP", "START", -1, "Failed start server", "", "", "", "", nil)
	}
}

package config

import (
	"encoding/json"
	"log"
	"os"
	"path/filepath"
	"reflect"
	"strconv"

	"github.com/joho/godotenv"
)

// Config ...
type Config struct {
	ServerPort            int    `json:"serverPort"`
	ReleaseMode           bool   `json:"releaseMode"`
	ServiceTimeoutSeconds int    `json:"serviceTimeoutSeconds"`
	LogLevel              string `json:"logLevel"`
	LogServiceRequests    bool   `json:"logServiceRequests"`
	CompraUrl             string `json:"compraUrl"`
	ESURL                 string `json:"ESURL"`
	CompraAuthToken       string `json:"compraAuthToken"`
}

func (c *Config) readFromFile(path string) {
	file, err := os.Open(path)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	err = json.NewDecoder(file).Decode(&c)
	if err != nil {
		log.Fatal(err)
	}
}

// New ...
func New(path string) *Config {
	var config Config
	config.readFromFile(path)
	envFile := filepath.Join(filepath.Dir(path), ".env")
	if _, err := os.Stat(envFile); !os.IsNotExist(err) {
		err := godotenv.Load(envFile)
		if err != nil {
			log.Fatal("Error loading .env file")
		}
		config.mapEnv()
	}
	return &config
}

func (c *Config) mapEnv() {
	v := reflect.ValueOf(c).Elem()
	t := v.Type()

	for i := 0; i < v.NumField(); i++ {
		f := v.Field(i)
		value := os.Getenv(t.Field(i).Name)
		if value != "" {
			switch f.Kind() {

			case reflect.String:
				f.SetString(value)

			case reflect.Int, reflect.Int8, reflect.Int16, reflect.Int32, reflect.Int64:
				i, err := strconv.ParseInt(value, 10, f.Type().Bits())
				if err == nil {
					if !f.OverflowInt(i) {
						f.SetInt(i)
					}
				}

			case reflect.Bool:
				b, err := strconv.ParseBool(value)
				if err == nil {
					f.SetBool(b)
				}

			case reflect.Uint, reflect.Uint8, reflect.Uint16, reflect.Uint32, reflect.Uint64, reflect.Uintptr:
				ui, err := strconv.ParseUint(value, 10, f.Type().Bits())
				if err == nil {
					if !f.OverflowUint(ui) {
						f.SetUint(ui)
					}
				}

			case reflect.Float32, reflect.Float64:
				fl, err := strconv.ParseFloat(value, f.Type().Bits())
				if err == nil {
					if !f.OverflowFloat(fl) {
						f.SetFloat(fl)
					}
				}

			default:
			}
		}
	}
}

{
  "serverPort": 8080,
  "releaseMode": true,
  "serviceTimeoutSeconds": 120,
  "logLevel": "DEBUG",
  "logServiceRequests": false,
  "compraUrl": "https://kompra.kz/api/",
  "ESURL": "http://10.3.41.13:9200/bp-api/_doc",
  "compraAuthToken": "test_API_v2"
}

package router

import (
	"github.com/gin-gonic/gin"
	ginSwagger "github.com/swaggo/gin-swagger"
	"github.com/swaggo/gin-swagger/swaggerFiles"
	_ "halykbpm-git.homebank.kz/business-processes/compraIP/docs"
	"halykbpm-git.homebank.kz/business-processes/compraIP/handler"
	"io/ioutil"
)

// InitRouter initialize routing information
func InitRouter(h *handler.Handler, production bool) *gin.Engine {
	if production {
		gin.SetMode(gin.ReleaseMode)
		gin.DefaultWriter = ioutil.Discard
	}
	r := gin.Default()
	r.GET("/")
	if !production {
		r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	}
	r.GET("/getTaxes", h.GetTaxes)
	r.GET("/healthz")

	return r
}

package handler

import (
	"github.com/gin-gonic/gin"
	log "halykbpm-git.homebank.kz/HomeBank/elastic-logger.v3"
	"halykbpm-git.homebank.kz/business-processes/compraIP/config"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
	"halykbpm-git.homebank.kz/business-processes/compraIP/render"
	"halykbpm-git.homebank.kz/business-processes/compraIP/repository"
	"net/http"
	"net/http/httputil"
	"strings"
)

type resourcePaths struct {
	Value string `uri:"value" binding:"required"`
	Param string `uri:"param" binding:"required"`
}

//Handler ...
type Handler struct {
	store  repository.Store
	config *config.Config
	logger log.Logger
}

//NewHandler ...
func NewHandler(store repository.Store, config *config.Config, logger log.Logger) *Handler {
	return &Handler{
		store:  store,
		config: config,
		logger: logger,
	}
}

func (h *Handler) needXML(c *gin.Context) bool {
	accept := c.Request.Header.Get("Accept")
	return strings.Contains(accept, "/xml")
}

func (h *Handler) ok(c *gin.Context, resp interface{}, fields ...string) {
	if h.needXML(c) {
		c.XML(http.StatusOK, &resp)
	} else {
		if len(fields) > 0 && fields[0] != "" {
			c.Render(http.StatusOK, render.JSON{Data: resp, Fields: fields[0]})
		} else {
			c.JSON(http.StatusOK, &resp)
		}
	}
}

func (h *Handler) fail(c *gin.Context, status int, err error) {
	buf, _ := httputil.DumpRequest(c.Request, true)
	h.logger.Error("compraIP", "Error handler", status, err.Error(), "", "", "", string(buf), nil)
	if h.needXML(c) {
		c.XML(status, &model.ErrorMessage{
			Code:    -1,
			Message: err.Error(),
		})
	} else {
		c.JSON(status, &model.ErrorMessage{
			Code:    -1,
			Message: err.Error(),
		})
	}
}

func (h *Handler) success(c *gin.Context, resp interface{}, fields ...string) {
	if len(fields) > 0 && fields[0] != "" {
		c.Render(http.StatusOK, render.JSON{Data: resp, Fields: fields[0]})
	} else {
		c.JSON(http.StatusOK, &resp)
	}
}

package handler

import (
	"context"
	"fmt"
	"github.com/gin-gonic/gin"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
	"halykbpm-git.homebank.kz/business-processes/compraIP/util"
	"net/http"
)

func (h *Handler) GetTaxes(c *gin.Context) {
	var paths resourcePaths
	if err := c.ShouldBindUri(&paths); err != nil {
		h.fail(c, http.StatusBadRequest, err)
		return
	}
	var req model.Client
	ok := util.BindRequest(&req, paths.Param, paths.Value)
	if !ok {
		h.fail(c, http.StatusBadRequest, fmt.Errorf("Invalid path param %s", paths.Param))
		return
	}

	resp, err := h.store.TaxesRepo().GetTaxes(context.Background(), &req)
	if err != nil {
		h.fail(c, http.StatusUnprocessableEntity, err)
		return
	}

	h.ok(c, &resp)
}

package model

type Client struct {
	IinBIN string `json:"iinBIN"`
}

package model

import (
	"encoding/xml"
	"fmt"

	sr "halykbpm-git.homebank.kz/business-processes/service.response"
)

// ErrorMessage ...
type ErrorMessage struct {
	XMLName xml.Name `xml:"Error" json:"-"`
	Code    int      `xml:"Code" json:"code"`
	Message string   `xml:"Message" json:"message"`
}

// FailMessage ...
type FailMessage struct {
	RetCode int    `xml:"RetCode,omitempty" json:"RetCode,omitempty"`
	RetMsg  string `xml:"RetMsg,omitempty" json:"RetMsg,omitempty"`
}

//APIError ...
type APIError struct {
	code    int
	message string
}

//CustomMessage -
type CustomMessage struct {
	Code    int    `json:"code"`
	Message string `json:"message"`
}

type CustomErrorWithData struct {
	IsSuccess bool        `json:"isSuccess"`
	Code      int         `json:"code"`
	Message   string      `json:"message"`
	Data      interface{} `json:"data"`
}

//Error ..
func (err APIError) Error() string {
	return fmt.Sprintf("%d - %s", err.code, err.message)
}

//Code ...
func (err *APIError) Code() int {
	return err.code
}

//Message  ...
func (err *APIError) Message() string {
	return err.message
}

//NewAPIError ...
func NewAPIError(code int, message string) error {
	return APIError{code: code, message: message}
}

//Error - объект для сообщения об ошибке
type Error struct {
	Code    int
	Message string
	Body    string
}

//NewError -
func NewError(code int, message, body string) error {
	return &Error{
		Code:    code,
		Message: message,
		Body:    body,
	}
}

func (e *Error) Error() string {
	return fmt.Sprintf("Error: %v - %v", e.Code, e.Message)
}

type CustomError struct {
	sr.CustomError
	Data    interface{} `json:"data"`
	RetCode int         `json:"retCode"`
}

func (c *CustomError) OkWithData(msg string, data interface{}) *CustomError {
	c.CustomError = *sr.Ok(msg)
	c.Data = data
	return c
}

func (c *CustomError) Error(code int, message string) *CustomError {
	c.CustomError = *sr.Error(code, message)
	return c
}

package model

type Taxes struct {
	Status      bool      `json:"status"`
	Total       float64   `json:"total"`
	LastUpdated int       `json:"lastUpdated"`
	Content     []Content `json:"content"`
}

type Content struct {
	Year   int     `json:"year"`
	Amount float64 `json:"amount"`
}

package repository

// Store ...
type Store interface {
	TaxesRepo() TaxesRepository
}

package repository

import (
	"context"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
)

// TaxesRepository ...
type TaxesRepository interface {
	GetTaxes(ctx context.Context, req *model.Client) (*model.Taxes, error)
}

package taxes

import (
	"halykbpm-git.homebank.kz/business-processes/compraIP/repository"
	rest "halykbpm-git.homebank.kz/business-processes/compraIP/service"
)

// Store ...
type Store struct {
	service   rest.Service
	taxesRepo *TaxesRepo
}

// New ...
func New(restService rest.Service) *Store {
	return &Store{
		service: restService,
	}
}

// Service ...
func (s *Store) TaxesRepo() repository.TaxesRepository {
	if s.taxesRepo != nil {
		return s.taxesRepo
	}

	s.taxesRepo = &TaxesRepo{
		store: s,
	}

	return s.taxesRepo
}

package taxes

import (
	"context"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
)

type TaxesRepo struct {
	store *Store
}

func (r *TaxesRepo) GetTaxes(ctx context.Context, req *model.Client) (*model.Taxes, error) {
	resp, err := r.store.service.GetTaxes(ctx, req)
	if err != nil {
		return nil, err
	}
	return resp, nil
}

package service

import (
	"context"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
)

type Service interface {
	GetTaxes(ctx context.Context, req *model.Client) (*model.Taxes, error)
}

package service

import (
	"context"
	"encoding/json"
	"github.com/pkg/errors"
	logger "halykbpm-git.homebank.kz/HomeBank/elastic-logger.v3"
	"halykbpm-git.homebank.kz/business-processes/compraIP/config"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
	"io"
	"io/ioutil"
	"net"
	"net/http"
	"net/url"
	"path"
	"time"
)

type options struct {
	timeout time.Duration
	client  HTTPClient
}

var defaultOptions = options{
	timeout: time.Duration(55 * time.Second),
}

type Option func(*options)

func WithHTTPClient(c HTTPClient) Option {
	return func(o *options) {
		o.client = c
	}
}

type CompraService struct {
	opts *options
	log  logger.Logger
	conf *config.Config
}

type HTTPClient interface {
	Do(req *http.Request) (*http.Response, error)
}

func NewCompraService(log logger.Logger, cfg *config.Config, opt ...Option) *CompraService {
	opts := defaultOptions
	for _, o := range opt {
		o(&opts)
	}
	return &CompraService{
		opts: &opts,
		log:  log,
		conf: cfg,
	}
}

func (c *CompraService) GetTaxes(ctx context.Context, req *model.Client) (*model.Taxes, error) {

	action := "GetTaxes"
	u, err := url.Parse(c.conf.CompraUrl)
	u.Path = path.Join(u.Path, "/taxes?identifier=", req.IinBIN, "&api-token=", c.conf.CompraAuthToken)
	qs := u.Query()
	u.RawQuery = qs.Encode()
	res, err := c.newRequest(ctx, "GET", u.String(), nil)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}
	defer res.Body.Close()

	body, err := ioutil.ReadAll(res.Body)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	var respBody *model.Taxes
	if err := json.Unmarshal(body, &respBody); err != nil {
		return nil, c.getError(err, action, u.String(), body)
	}

	return respBody, nil
}

func (c *CompraService) newRequest(ctx context.Context, method, _url string, body io.Reader) (*http.Response, error) {
	req, err := http.NewRequest(method, _url, body)
	if err != nil {
		return nil, err
	}

	req.Header.Set("content-type", "application/json")
	req = req.WithContext(ctx)

	client := c.opts.client
	if client == nil {
		tr := &http.Transport{
			DialContext: func(ctx context.Context, network, addr string) (net.Conn, error) {
				d := net.Dialer{Timeout: 55 * time.Second}
				return d.DialContext(ctx, network, addr)
			},
		}
		client = &http.Client{Timeout: 55 * time.Second, Transport: tr}
	}

	return client.Do(req)
}

func (c *CompraService) getError(err error, action, path string, body []byte) error {
	var fail model.FailMessage
	errf := json.Unmarshal(body, &fail)
	if errf != nil {
		c.log.Error("compra.integration", action, -1, "Parse Response Error", path, "", "", string(body), nil)
		return err
	}
	return model.NewAPIError(fail.RetCode, fail.RetMsg)
}

package logger

import (
	"log"
	"net/http"
	"net/http/httputil"
	"time"
)

type loggedRoundTripper struct {
	rt  http.RoundTripper
	log HTTPLogger
}

func NewLoggedRoundTripper(rt http.RoundTripper,
	log HTTPLogger) *loggedRoundTripper {
	return &loggedRoundTripper{rt: rt, log: log}
}

func (lrt *loggedRoundTripper) RoundTrip(request *http.Request) (*http.Response, error) {
	lrt.log.logRequest(request)
	startTime := time.Now()
	response, err := lrt.rt.RoundTrip(request)
	duration := time.Since(startTime)
	lrt.log.logResponse(request, response, err, duration)
	return response, err
}

// HTTPLogger defines the interface to log http request and responses
type HTTPLogger interface {
	logRequest(*http.Request)
	logResponse(*http.Request, *http.Response, error, time.Duration)
}

type defaultLogger struct {
}

func NewDefaultLogger() *defaultLogger {
	return &defaultLogger{}
}

// LogRequest -
func (dl *defaultLogger) logRequest(req *http.Request) {
	buf, err := httputil.DumpRequestOut(req, true)
	if err != nil {
		log.Printf("Error dump HTTP Request: %s", err.Error())
		return
	}
	log.Printf("HTTP Request: %s", string(buf[:]))
}

// LogResponse -
func (dl *defaultLogger) logResponse(req *http.Request, res *http.Response, err error, duration time.Duration) {
	duration /= time.Millisecond
	if err != nil {
		log.Printf("HTTP Request method=%s host=%s path=%s duration=%d status=error error=%q", req.Method, req.Host, req.URL.Path, duration, err.Error())
	} else {
		buf, err := httputil.DumpResponse(res, true)
		if err != nil {
			log.Printf("Error dump HTTP Response: %s", err.Error())
			return
		}
		log.Printf("HTTP Response: %s duration: %d", string(buf), duration)
	}
}

package logger

import (
	logger "halykbpm-git.homebank.kz/HomeBank/elastic-logger.v3"
	"log"
	"os"
)

type logWrapper struct {
	log   logger.Logger
	level string
	mode  string
}

//NewLogger ...
func NewLogger(url string, level string) *logWrapper {
	l := logger.NewLogger(url)
	if level != "" {
		if err := l.SetLevel(level); err != nil {
			log.Fatalf("Error Set Logger Level: %v", err)
		}
	}

	return &logWrapper{
		log:   l,
		level: l.GetLevel(),
		mode:  os.Getenv("APP_MODE"),
	}
}

//Tracef полный формат трассирующего лога
func (l logWrapper) Tracef(component, action string, code int, message, customMessage, userID, clientID string, data interface{}, additionalFields *map[string]interface{}) {
	if l.mode == "development" {
		l.toStdout("TRACE", component, action, code, message, customMessage, userID, clientID, data, additionalFields)
		return
	}
	go l.log.Tracef(component, action, code, message, customMessage, userID, clientID, data, additionalFields)
}

func (l logWrapper) Traces(component, action string, message, customMessage string, additionalFields *map[string]interface{}) {
	if l.mode == "development" {
		l.toStdout("TRACE", component, action, 0, message, customMessage, "", "", nil, additionalFields)
		return
	}
	go l.log.Traces(component, action, message, customMessage, additionalFields)
}

func (l logWrapper) Debug(component, action string, message, customMessage, userID, clientID string, data interface{}, additionalFields *map[string]interface{}) {
	if l.mode == "development" {
		l.toStdout("DEBUG", component, action, 0, message, customMessage, userID, clientID, data, additionalFields)
		return
	}
	go l.log.Debug(component, action, message, customMessage, userID, clientID, data, additionalFields)
}

func (l logWrapper) Info(component, action, message, customMessage, userID, clientID string, data interface{}, additionalFields *map[string]interface{}) {
	if l.mode == "development" {
		l.toStdout("INFO", component, action, 0, message, customMessage, userID, clientID, data, additionalFields)
		return
	}
	go l.log.Info(component, action, message, customMessage, userID, clientID, data, additionalFields)
}

func (l logWrapper) Warn(component, action, message, customMessage, userID, clientID string, data interface{}, additionalFields *map[string]interface{}) {
	if l.mode == "development" {
		l.toStdout("WARN", component, action, 0, message, customMessage, userID, clientID, data, additionalFields)
		return
	}
	go l.log.Warn(component, action, message, customMessage, userID, clientID, data, additionalFields)
}

func (l logWrapper) Error(component, action string, code int, message, customMessage, userID, clientID string, data interface{}, additionalFields *map[string]interface{}) {
	if l.mode == "development" {
		l.toStdout("ERROR", component, action, code, message, customMessage, userID, clientID, data, additionalFields)
		return
	}
	go l.log.Error(component, action, code, message, customMessage, userID, clientID, data, additionalFields)
}

func (l logWrapper) Fatal(component, action string, code int, message, customMessage, userID, clientID string, data interface{}, additionalFields *map[string]interface{}) {
	if l.mode == "development" {
		log.Fatalf("%s | %s | %d | %s | %s | %s | %s | %+v | %+v\n", component, action, code, message, customMessage, userID, clientID, data, additionalFields)
	}
	l.log.Fatal(component, action, code, message, customMessage, userID, clientID, data, additionalFields)
}

func (l logWrapper) toStdout(level, component, action string, code int, message, customMessage, userID, clientID string, data interface{}, additionalFields *map[string]interface{}) {
	log.Printf("[%s] %s | %s | %d | %s | %s | %s | %s | %+v | %+v\n", level, component, action, code, message, customMessage, userID, clientID, data, additionalFields)
}

package util

import (
	"reflect"
	"strings"
)

//BindRequest ...
func BindRequest(dest interface{}, param string, value string) bool {
	v := reflect.ValueOf(dest).Elem()
	t := v.Type()
	ok := false
	for i := 0; i < t.NumField(); i++ {
		if t.Field(i).PkgPath != "" {
			continue
		}
		nameField := v.Type().Field(i).Name
		if v.Field(i).Kind() == reflect.String && strings.ToLower(nameField) == strings.ToLower(param) {
			v.Field(i).SetString(value)
			ok = true
		}
	}
	return ok
}

module halykbpm-git.homebank.kz/business-processes/compraIP

go 1.17

require (
	github.com/alecthomas/template v0.0.0-20190718012654-fb15b899a751
	github.com/gin-gonic/gin v1.7.7
	github.com/joho/godotenv v1.4.0
	github.com/pkg/errors v0.9.1
	github.com/swaggo/gin-swagger v1.3.3
	github.com/swaggo/swag v1.7.4
	halykbpm-git.homebank.kz/HomeBank/elastic-logger.v3 v0.0.0-20211214060908-8d08b31ee879
	halykbpm-git.homebank.kz/business-processes/service.response v1.0.0
)

require (
	github.com/KyleBanks/depth v1.2.1 // indirect
	github.com/PuerkitoBio/purell v1.1.1 // indirect
	github.com/PuerkitoBio/urlesc v0.0.0-20170810143723-de5bf2ad4578 // indirect
	github.com/gabriel-vasile/mimetype v1.4.2 // indirect
	github.com/gin-contrib/sse v0.1.0 // indirect
	github.com/go-openapi/jsonpointer v0.19.5 // indirect
	github.com/go-openapi/jsonreference v0.19.5 // indirect
	github.com/go-openapi/spec v0.20.3 // indirect
	github.com/go-openapi/swag v0.19.14 // indirect
	github.com/go-playground/locales v0.14.1 // indirect
	github.com/go-playground/universal-translator v0.18.1 // indirect
	github.com/go-playground/validator/v10 v10.14.0 // indirect
	github.com/golang/protobuf v1.5.2 // indirect
	github.com/josharian/intern v1.0.0 // indirect
	github.com/json-iterator/go v1.1.12 // indirect
	github.com/leodido/go-urn v1.2.4 // indirect
	github.com/mailru/easyjson v0.7.6 // indirect
	github.com/mattn/go-isatty v0.0.19 // indirect
	github.com/modern-go/concurrent v0.0.0-20180306012644-bacd9c7ef1dd // indirect
	github.com/modern-go/reflect2 v1.0.2 // indirect
	github.com/stretchr/testify v1.8.3 // indirect
	github.com/ugorji/go/codec v1.2.11 // indirect
	golang.org/x/crypto v0.9.0 // indirect
	golang.org/x/net v0.10.0 // indirect
	golang.org/x/sys v0.8.0 // indirect
	golang.org/x/text v0.9.0 // indirect
	golang.org/x/tools v0.6.0 // indirect
	google.golang.org/protobuf v1.30.0 // indirect
	gopkg.in/yaml.v2 v2.4.0 // indirect
	halykbpm-git.homebank.kz/EPAY2/luhn v0.0.0-20200618124840-d046ec9137c9 // indirect
)

// GENERATED BY THE COMMAND ABOVE; DO NOT EDIT
// This file was generated by swaggo/swag at
// 2024-02-27 08:56:03.1537605 +0600 +06 m=+0.092001701

package docs

import (
	"bytes"
	"encoding/json"
	"strings"

	"github.com/alecthomas/template"
	"github.com/swaggo/swag"
)

var doc = `{
    "schemes": {{ marshal .Schemes }},
    "swagger": "2.0",
    "info": {
        "description": "{{.Description}}",
        "title": "{{.Title}}",
        "contact": {},
        "license": {},
        "version": "{{.Version}}"
    },
    "host": "{{.Host}}",
    "basePath": "{{.BasePath}}",
    "paths": {}
}`

type swaggerInfo struct {
	Version     string
	Host        string
	BasePath    string
	Schemes     []string
	Title       string
	Description string
}

// SwaggerInfo holds exported Swagger Info so clients can modify it
var SwaggerInfo = swaggerInfo{
	Version:     "",
	Host:        "",
	BasePath:    "",
	Schemes:     []string{},
	Title:       "",
	Description: "",
}

type s struct{}

func (s *s) ReadDoc() string {
	sInfo := SwaggerInfo
	sInfo.Description = strings.Replace(sInfo.Description, "\n", "\\n", -1)

	t, err := template.New("swagger_info").Funcs(template.FuncMap{
		"marshal": func(v interface{}) string {
			a, _ := json.Marshal(v)
			return string(a)
		},
	}).Parse(doc)
	if err != nil {
		return doc
	}

	var tpl bytes.Buffer
	if err := t.Execute(&tpl, sInfo); err != nil {
		return doc
	}

	return tpl.String()
}

func init() {
	swag.Register(swag.Name, &s{})
}

package render

import (
	"fmt"
	"net/http"
	"reflect"
	"strings"

	"encoding/json"
)

// JSON contains the given interface object.
type JSON struct {
	Data   interface{}
	Fields string
}

var jsonContentType = []string{"application/json; charset=utf-8"}

// Render (JSON) writes data with custom ContentType.
func (r JSON) Render(w http.ResponseWriter) (err error) {
	if err = WriteJSON(w, r.Data, r.Fields); err != nil {
		panic(err)
	}
	return
}

// WriteContentType (JSON) writes JSON ContentType.
func (r JSON) WriteContentType(w http.ResponseWriter) {
	writeContentType(w, jsonContentType)
}

// WriteJSON marshals the given interface object and writes it with custom ContentType.
func WriteJSON(w http.ResponseWriter, obj interface{}, fields string) error {
	writeContentType(w, jsonContentType)
	encoder := json.NewEncoder(w)
	fs, err := filter(obj, fields)
	if err != nil {
		return err
	}
	return encoder.Encode(fs)
}

func writeContentType(w http.ResponseWriter, value []string) {
	header := w.Header()
	if val := header["Content-Type"]; len(val) == 0 {
		header["Content-Type"] = value
	}
}

func fieldSet(fields string) map[string]bool {
	if fields == "" {
		return nil
	}
	f := strings.Split(fields, ",")
	set := make(map[string]bool, len(f))
	for _, s := range f {
		set[s] = true
	}
	return set
}

func selectFields(obj interface{}, fields string) (interface{}, error) {
	rt := reflect.TypeOf(obj)
	var v reflect.Value
	if rt.Kind() == reflect.Ptr {
		v = reflect.ValueOf(obj).Elem()
	} else {
		v = reflect.ValueOf(obj)
	}
	t := v.Type()
	if t.Kind() == reflect.Struct {
		return selectFieldsStruct(v, fields), nil
	} else if t.Kind() == reflect.Slice {
		s := reflect.ValueOf(obj)
		m := make([]map[string]interface{}, 0, s.Len())
		for i := 0; i < s.Len(); i++ {
			var v reflect.Value
			if s.Index(i).Type().Kind() == reflect.Ptr {
				v = s.Index(i).Elem()
			} else {
				v = s.Index(i)
			}
			m = append(m, selectFieldsStruct(v, fields))
		}
		return m, nil
	} else {
		return nil, fmt.Errorf("Usupported type %d", rt.Kind())
	}

}

func selectFieldsStruct(v reflect.Value, fields string) map[string]interface{} {
	fs := fieldSet(fields)
	t := v.Type()
	out := make(map[string]interface{}, t.NumField())
	for i := 0; i < t.NumField(); i++ {
		field := t.Field(i)
		key := strings.Split(field.Tag.Get("json"), ",")[0]
		if fs == nil || fs[key] {
			out[key] = v.Field(i).Interface()
		}
	}
	return out
}

func filter(obj interface{}, fields string) (map[string]interface{}, error) {
	rt := reflect.TypeOf(obj)
	var v reflect.Value
	if rt.Kind() == reflect.Ptr {
		v = reflect.ValueOf(obj).Elem()
	} else {
		v = reflect.ValueOf(obj)
	}
	t := v.Type()
	if t.Kind() == reflect.Struct {
		out := make(map[string]interface{}, t.NumField())
		var err error
		for i := 0; i < t.NumField(); i++ {
			field := t.Field(i)
			key := strings.Split(field.Tag.Get("json"), ",")[0]
			if key == "-" {
				continue
			}

			if key == "" {
				status := selectFieldsStruct(v.Field(i), "")
				for k, v := range status {
					out[k] = v
				}

			} else if key == "data" {
				out[key], err = selectFields(v.Field(i).Interface(), fields)
				if err != nil {
					return nil, err
				}
			} else {
				out[key] = v.Field(i).Interface()
			}
		}
		return out, nil
	} else {
		return nil, fmt.Errorf("Usupported type %d", rt.Kind())
	}
}
