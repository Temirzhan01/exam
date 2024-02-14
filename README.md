package main

import "io/ioutil"
import "net/http"
import "log"
import "encoding/json"
import "fmt"

type Taxes struct {
	status      bool
	total       float64
	lastUpdated string
	content     []Content
}

type Content struct {
	year   int
	amount float64
}

func main() {
	resp, err := http.Get("urlHere")
	if err != nil {
		log.Fatalf("Cant create request: %v", err)
	}
	defer resp.Body.Close()

	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		log.Fatalf("Can't read response: %v", err)
	}
	var respBody Taxes
	if err := json.Unmarshal(body, respBody); err != nil {
		log.Fatalf("Can't unmarshal response body: %v", err)
	}

	fmt.Printf("Status: %t, Total: %f, LastUpdated: %s, Content: %v", respBody.status, respBody.total, respBody.lastUpdated, respBody.content)
}


Cant create request: Get "urlHere": dial tcp 94.247.131.230:443: connectex: A connection attempt failed because the connected party did not
 properly respond after a period of time, or established connection failed because connected host has failed to respond.
exit status 1
