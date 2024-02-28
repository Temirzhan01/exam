package service

import (
	"context"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
)

type Service interface {
	CheckIP(client string) (string, error)
	GetResults(refer string) ([]*model.Row, error)
}

type Integration interface {
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

type IntegraionService struct {
	opts *options
	log  logger.Logger
	conf *config.Config
}

type HTTPClient interface {
	Do(req *http.Request) (*http.Response, error)
}

func NewIntegraionService(log logger.Logger, cfg *config.Config, opt ...Option) *IntegraionService {
	opts := defaultOptions
	for _, o := range opt {
		o(&opts)
	}
	return &IntegraionService{
		opts: &opts,
		log:  log,
		conf: cfg,
	}
}

func (is *IntegraionService) GetTaxes(ctx context.Context, req *model.Client) (*model.Taxes, error) {

	action := "GetTaxes"
	u, err := url.Parse(is.conf.CompraUrl)
	u.Path = path.Join(u.Path, "/compra?identifier=", req.IinBIN, "&api-token=", is.conf.CompraAuthToken)
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

	var respBody *model.Taxes
	if err := json.Unmarshal(body, &respBody); err != nil {
		return nil, c.getError(err, action, u.String(), body)
	}

	return respBody, nil
}

func (is *IntegraionService) newRequest(ctx context.Context, method, _url string, body io.Reader) (*http.Response, error) {
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

func (is *IntegraionService) getError(err error, action, path string, body []byte) error {
	var fail model.FailMessage
	errf := json.Unmarshal(body, &fail)
	if errf != nil {
		is.log.Error("compra.integration", action, -1, "Parse Response Error", path, "", "", string(body), nil)
		return err
	}
	return model.NewAPIError(fail.RetCode, fail.RetMsg)
}



package service

import (
	"github.com/google/uuid"
	"halykbpm-git.homebank.kz/business-processes/compraIP/model"
)

type CompraService struct {
	IntegraionService *IntegraionService
}

func (s *CompraService) CheckIP(client string) (string, error) {
	guid := uuid.New().String()
	entry := model.Row{IinBIN: client, Refer: guid, Status: false, Result: false}
	return entry.IinBIN, nil
}

func (s *CompraService) GetResults(refer string) ([]model.Row, error) {
	var rows []model.Row
	return rows, nil
}
