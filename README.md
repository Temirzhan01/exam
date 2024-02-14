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
	resp, err := http.Get("https://kompra.kz/api/v2/taxes?identifier=140241009172&api-token=test_API_v2")
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


Can't unmarshal response body: json: Unmarshal(non-pointer main.Taxes)
exit status 1
