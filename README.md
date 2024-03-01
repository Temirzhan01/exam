package main

import (
	"fmt"
	"halykbpm-git.homebank.kz/business-processes/compraIP/app"
	"halykbpm-git.homebank.kz/business-processes/compraIP/config"
	"halykbpm-git.homebank.kz/business-processes/compraIP/docs"
	"os"
)

func main() {
	configs := config.New("config.json")
	if os.Getenv("APP_MODE") == "development" {
		docs.SwaggerInfo.Host = fmt.Sprintf("localhost:%d", configs.ServerPort)
	}
	app.Start(configs)
}

module halykbpm-git.homebank.kz/business-processes/compraIP

go 1.17

require (
	github.com/alecthomas/template v0.0.0-20190718012654-fb15b899a751
	github.com/gin-gonic/gin v1.9.0
	github.com/google/uuid v1.3.0
	github.com/jinzhu/gorm v1.9.16
	github.com/pkg/errors v0.9.1
	github.com/spf13/viper v1.9.0
	github.com/swaggo/gin-swagger v1.3.3
	github.com/swaggo/swag v1.7.4
	halykbpm-git.homebank.kz/HomeBank/elastic-logger.v3 v0.0.0-20211214060908-8d08b31ee879
	halykbpm-git.homebank.kz/business-processes/service.response v1.0.0
)

require (
	github.com/KyleBanks/depth v1.2.1 // indirect
	github.com/PuerkitoBio/purell v1.1.1 // indirect
	github.com/PuerkitoBio/urlesc v0.0.0-20170810143723-de5bf2ad4578 // indirect
	github.com/asaskevich/govalidator v0.0.0-20230301143203-a9d515a09cc2 // indirect
	github.com/bytedance/sonic v1.8.0 // indirect
	github.com/chenzhuoyu/base64x v0.0.0-20221115062448-fe3a3abad311 // indirect
	github.com/dgrijalva/jwt-go v3.2.0+incompatible // indirect
	github.com/fsnotify/fsnotify v1.5.1 // indirect
	github.com/gabriel-vasile/mimetype v1.4.2 // indirect
	github.com/gin-contrib/sse v0.1.0 // indirect
	github.com/go-openapi/errors v0.20.3 // indirect
	github.com/go-openapi/jsonpointer v0.19.5 // indirect
	github.com/go-openapi/jsonreference v0.19.5 // indirect
	github.com/go-openapi/spec v0.20.3 // indirect
	github.com/go-openapi/strfmt v0.21.7 // indirect
	github.com/go-openapi/swag v0.19.14 // indirect
	github.com/go-playground/locales v0.14.1 // indirect
	github.com/go-playground/universal-translator v0.18.1 // indirect
	github.com/go-playground/validator/v10 v10.14.0 // indirect
	github.com/goccy/go-json v0.10.0 // indirect
	github.com/hashicorp/hcl v1.0.0 // indirect
	github.com/jinzhu/inflection v1.0.0 // indirect
	github.com/josharian/intern v1.0.0 // indirect
	github.com/json-iterator/go v1.1.12 // indirect
	github.com/klauspost/cpuid/v2 v2.0.9 // indirect
	github.com/leodido/go-urn v1.2.4 // indirect
	github.com/lib/pq v1.1.1 // indirect
	github.com/magiconair/properties v1.8.5 // indirect
	github.com/mailru/easyjson v0.7.6 // indirect
	github.com/mattn/go-isatty v0.0.19 // indirect
	github.com/mitchellh/mapstructure v1.5.0 // indirect
	github.com/modern-go/concurrent v0.0.0-20180306012644-bacd9c7ef1dd // indirect
	github.com/modern-go/reflect2 v1.0.2 // indirect
	github.com/oklog/ulid v1.3.1 // indirect
	github.com/pelletier/go-toml v1.9.4 // indirect
	github.com/pelletier/go-toml/v2 v2.0.6 // indirect
	github.com/spf13/afero v1.6.0 // indirect
	github.com/spf13/cast v1.4.1 // indirect
	github.com/spf13/jwalterweatherman v1.1.0 // indirect
	github.com/spf13/pflag v1.0.5 // indirect
	github.com/stretchr/testify v1.8.3 // indirect
	github.com/subosito/gotenv v1.2.0 // indirect
	github.com/twitchyliquid64/golang-asm v0.15.1 // indirect
	github.com/ugorji/go/codec v1.2.11 // indirect
	go.mongodb.org/mongo-driver v1.11.3 // indirect
	golang.org/x/arch v0.0.0-20210923205945-b76863e36670 // indirect
	golang.org/x/crypto v0.9.0 // indirect
	golang.org/x/net v0.10.0 // indirect
	golang.org/x/sys v0.8.0 // indirect
	golang.org/x/text v0.9.0 // indirect
	golang.org/x/tools v0.6.0 // indirect
	google.golang.org/protobuf v1.30.0 // indirect
	gopkg.in/ini.v1 v1.63.2 // indirect
	gopkg.in/yaml.v2 v2.4.0 // indirect
	gopkg.in/yaml.v3 v3.0.1 // indirect
	halykbpm-git.homebank.kz/EPAY2/luhn v0.0.0-20200618124840-d046ec9137c9 // indirect
	halykbpm-git.homebank.kz/business-processes/common v1.2.8 // indirect
)

package app

import (
	"fmt"
	"net"
	"net/http"
	"time"

	"halykbpm-git.homebank.kz/business-processes/compraIP/config"
	"halykbpm-git.homebank.kz/business-processes/compraIP/handler"
	"halykbpm-git.homebank.kz/business-processes/compraIP/repository"
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

	h := handler.NewHandler(repository.CompraRepository{repository.NewDb(config)}, service.CompraService{Integraion: service.NewIntegrationService(log, config, service.WithHTTPClient(client))}, config, log)
	r := router.InitRouter(h, config.ReleaseMode)

	log.Info("compraIP", "START", "Starting", "", "", "", "", nil)
	if err := r.Run(fmt.Sprintf(":%d", config.ServerPort)); err != nil {
		log.Fatal("compraIP", "START", -1, "Failed start server", "", "", "", "", nil)
	}
}

package config

import (
	"github.com/spf13/viper"
	"log"
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
	DbUser                string `json:"dbUser"`
	DbPass                string `json:"dbPass"`
	DbName                string `json:"dbName"`
	DbHost                string `json:"dbHost"`
	DbPort                string `json:"dbPort"`
	DbMaxIdleConns        int    `json:"dbMaxIdleConns"`
	DbMaxOpenConns        int    `json:"dbMaxOpenConns"`
	DbConnMaxLifetime     int    `json:"dbConnMaxLifetime"`
	DbLogMode             bool   `json:"dbLogMode"`
}

func New(path string) *Config {
	viper.SetConfigFile(path)
	viper.AutomaticEnv()

	if err := viper.ReadInConfig(); err != nil {
		log.Fatalf("Error reading config file, %s", err)
	}

	var config Config
	if err := viper.Unmarshal(&config); err != nil {
		log.Fatalf("Unable to decode into struct, %v", err)
	}

	return &config
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
	if !production {
		r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	}
	r.POST("/checkIP", h.CheckIP)
	r.POST("/getCheckResults", h.GetCheckResults)
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
	"halykbpm-git.homebank.kz/business-processes/compraIP/service"
	"net/http"
	"net/http/httputil"
)

type resourcePaths struct {
	Value string `uri:"value" binding:"required"`
	Param string `uri:"param" binding:"required"`
}

//Handler ...
type Handler struct {
	repository repository.IRepository
	service    service.Service
	config     *config.Config
	logger     log.Logger
}

//NewHandler ...
func NewHandler(repo repository.IRepository, service service.Service, config *config.Config, logger log.Logger) *Handler {
	return &Handler{
		repository: repo,
		service:    service,
		config:     config,
		logger:     logger,
	}
}

func (h *Handler) ok(c *gin.Context, resp interface{}, fields ...string) {
	if len(fields) > 0 && fields[0] != "" {
		c.Render(http.StatusOK, render.JSON{Data: resp, Fields: fields[0]})
	} else {
		c.JSON(http.StatusOK, &resp)
	}
}

func (h *Handler) fail(c *gin.Context, status int, err error) {
	buf, _ := httputil.DumpRequest(c.Request, true)
	h.logger.Error("compraIP", "Error handler", status, err.Error(), "", "", "", string(buf), nil)
	c.JSON(status, &model.ErrorMessage{
		Code:    -1,
		Message: err.Error(),
	})
}

package handler

import (
	"github.com/gin-gonic/gin"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
	"net/http"
)

func (h *Handler) CheckIP(c *gin.Context) {
	var client model.Client
	if err := c.ShouldBindUri(&client); err != nil {
		h.fail(c, http.StatusBadRequest, err)
		return
	}

	quid, err := h.service.CheckIP(&client)
	if err != nil {
		h.fail(c, http.StatusInternalServerError, err)
		return
	}

	h.ok(c, &quid)
}

func (h *Handler) GetCheckResults(c *gin.Context) {
	var refer model.Refer
	if err := c.ShouldBindUri(&refer); err != nil {
		h.fail(c, http.StatusBadRequest, err)
		return
	}

	results, err := h.service.GetResults(&refer)
	if err != nil {
		h.fail(c, http.StatusInternalServerError, err)
		return
	}

	h.ok(c, &results)
}

package service

import (
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
)

type Service interface {
	CheckIP(req *model.Client) (string, error)
	GetResults(refer *model.Refer) (*model.Row, error)
}

type Integration interface {
	GetRiskFactors(clientIINBIN string, isFirstReq bool) (*model.RiskFactorsResponse, error)
}

package service

import (
	"github.com/google/uuid"
	"github.com/pkg/errors"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
	"halykbpm-git.homebank.kz/business-processes/compraIP/repository"
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
	timeout: (55 * time.Second),
}

type Option func(*options)

func WithHTTPClient(c HTTPClient) Option {
	return func(o *options) {
		o.client = c
	}
}

type IntegrationService struct {
	opts *options
	log  logger.Logger
	conf *config.Config
}

type HTTPClient interface {
	Do(req *http.Request) (*http.Response, error)
}

func NewIntegrationService(log logger.Logger, cfg *config.Config, opt ...Option) *IntegrationService {
	opts := defaultOptions
	for _, o := range opt {
		o(&opts)
	}
	return &IntegrationService{
		opts: &opts,
		log:  log,
		conf: cfg,
	}
}

func (is *IntegrationService) GetRiskFactors(clientIINBIN string, isFirstReq bool) (*model.RiskFactorsResponse, error) {

	action := "GetRiskFactors"
	u, err := url.Parse(is.conf.CompraUrl)
	u.Path = path.Join(u.Path, "v2/reliability-list?identifier=", clientIINBIN, "&api-token=", is.conf.CompraAuthToken)
	qs := u.Query()
	u.RawQuery = qs.Encode()
	res, err := is.newRequest(ctx, "GET", u.String(), nil)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}
	defer res.Body.Close()

	body, err := ioutil.ReadAll(res.Body)
	if err != nil {
		return nil, errors.Wrapf(err, action)
	}

	var respBody *model.RiskFactorsResponse
	if isFirstReq {
		return nil, nil
	}
	if err := json.Unmarshal(body, &respBody); err != nil {
		return nil, is.getError(err, action, u.String(), body)
	}

	return respBody, nil
}

func (is *IntegrationService) newRequest(ctx context.Context, method, _url string, body io.Reader) (*http.Response, error) {
	req, err := http.NewRequest(method, _url, body)
	if err != nil {
		return nil, err
	}

	req.Header.Set("content-type", "application/json")
	req = req.WithContext(ctx)

	client := is.opts.client
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

func (is *IntegrationService) getError(err error, action, path string, body []byte) error {
	var fail model.FailMessage
	errf := json.Unmarshal(body, &fail)
	if errf != nil {
		is.log.Error("compra", action, -1, "Parse Response Error", path, "", "", string(body), nil)
		return err
	}
	return model.NewAPIError(fail.RetCode, fail.RetMsg)
}


package service

import (
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
	"time"
)

func ParseResults(response *model.RiskFactorsResponse, guid string, iin string, isFirst bool) (*model.Row, error) {
	row := model.Row{Refer: guid, Status: false, IinBIN: iin}
	if isFirst {
		return &row, nil
	}
	filtered := FilterResults(response)
	if IsReady(filtered) {
		for _, rf := range filtered.RiskFactors {
			res := model.Result{Type: rf.Type, Date: time.Unix(rf.LastUpdated, 0)}
			if rf.Status == "YES" {
				res.Status = true
			} else {
				res.Status = false
			}
			row.Result = append(row.Result, res)
		}
		row.Status = true
	}
	return &row, nil
}

func FilterResults(response *model.RiskFactorsResponse) *model.RiskFactorsResponse {
	var filtered *model.RiskFactorsResponse
	necessaryFactorsInd := []int{1, 2, 3, 4, 5, 6, 11, 13, 14, 16, 28, 32, 33, 34, 35, 36, 37, 38}
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

package repository

import (
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
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
	"halykbpm-git.homebank.kz/business-processes/compraIP/config"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
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
	db.AutoMigrate()
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

//Error ..
func (err APIError) Error() string {
	return fmt.Sprintf("%d - %s", err.code, err.message)
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

func (e *Error) Error() string {
	return fmt.Sprintf("Error: %v - %v", e.Code, e.Message)
}

type CustomError struct {
	sr.CustomError
	Data    interface{} `json:"data"`
	RetCode int         `json:"retCode"`
}

func (c *CustomError) Error(code int, message string) *CustomError {
	c.CustomError = *sr.Error(code, message)
	return c
}

package model

import "time"

type Row struct {
	Refer  string    `json:"refer"`
	Status bool      `json:"status"`
	IinBIN string    `json:"iinBIN"`
	Result []Result  `json:"result"`
	Date   time.Time `json:"date"`
}

type Result struct {
	Type   RiskFactorType `json:"type"`
	Status bool           `json:"status"`
	Date   time.Time      `json:"date"`
}

type Client struct {
	IinBIN string `json:"iinBIN"`
}

type Refer struct {
	Refer string `json:"refer"`
}

package model

type RiskFactorsResponse struct {
	RiskFactors []RiskFactor
}

type RiskFactor struct {
	ID          int            `json:"id"`
	Created     int64          `json:"created"`
	LastUpdated int64          `json:"lastUpdated"`
	Type        RiskFactorType `json:"type"`
	Rnn         string         `json:"rnn"`
	Bin         string         `json:"bin"`
	Iin         string         `json:"iin"`
	Content     Content        `json:"content"`
	Status      string         `json:"status"`
}

type RiskFactorType struct {
	Id   int    `json:"id"`
	Name string `json:"name"`
}

type Content struct {
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

# ---> Go
# Binaries for programs and plugins
*.exe
*.exe~
*.dll
*.so
*.dylib

# Test binary, built with `go test -c`
*.test

# Output of the go coverage tool, specifically when used with LiteIDE
*.out

# Dependency directories (remove the comment below to include it)
# vendor/

# ---> VisualStudioCode
.vscode/*
!.vscode/settings.json
!.vscode/tasks.json
!.vscode/launch.json
!.vscode/extensions.json
*.code-workspace

# ---> JetBrains
# Covers JetBrains IDEs: IntelliJ, RubyMine, PhpStorm, AppCode, PyCharm, CLion, Android Studio and WebStorm
# Reference: https://intellij-support.jetbrains.com/hc/en-us/articles/206544839

# User-specific stuff
.idea/**/workspace.xml
.idea/**/tasks.xml
.idea/**/usage.statistics.xml
.idea/**/dictionaries
.idea/**/shelf

# Generated files
.idea/**/contentModel.xml

# Sensitive or high-churn files
.idea/**/dataSources/
.idea/**/dataSources.ids
.idea/**/dataSources.local.xml
.idea/**/sqlDataSources.xml
.idea/**/dynamic.xml
.idea/**/uiDesigner.xml
.idea/**/dbnavigator.xml

# Gradle
.idea/**/gradle.xml
.idea/**/libraries

# Gradle and Maven with auto-import
# When using Gradle or Maven with auto-import, you should exclude module files,
# since they will be recreated, and may cause churn.  Uncomment if using
# auto-import.
# .idea/artifacts
# .idea/compiler.xml
# .idea/jarRepositories.xml
# .idea/modules.xml
# .idea/*.iml
# .idea/modules
# *.iml
# *.ipr

# CMake
cmake-build-*/

# Mongo Explorer plugin
.idea/**/mongoSettings.xml

# File-based project format
*.iws

# IntelliJ
out/

# mpeltonen/sbt-idea plugin
.idea_modules/

# JIRA plugin
atlassian-ide-plugin.xml

# Cursive Clojure plugin
.idea/replstate.xml

# Crashlytics plugin (for Android Studio and IntelliJ)
com_crashlytics_export_strings.xml
crashlytics.properties
crashlytics-build.properties
fabric.properties

# Editor-based Rest Client
.idea/httpRequests

# Android studio 3.1+ serialized cache file
.idea/caches/build_file_checksums.ser

.env

.idea/
vendor
