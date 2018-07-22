package main

import (
	"strconv"
	"strings"
	"sync"
)

type (
	Configer struct {
		sync.RWMutex
		files []string
		data  map[string]map[string]string
	}
)

func (this *Configer) set(key string, value string) {
	this.Lock()
	defer this.Unlock()
	if len(key) == 0 {
		return
	}
	var (
		sec, k string
		secKey = strings.Split(key, "::")
	)

	if len(secKey) >= 2 {
		sec = secKey[0]
		k = secKey[1]
	} else {
		sec = defaultSection
		k = secKey[0]
	}
	if _, ok := this.data[sec]; !ok {
		this.data[sec] = make(map[string]string)
	}
	this.data[sec][k] = value
}

func (this *Configer) get(key string) string {
	this.Lock()
	defer this.Unlock()
	if len(key) == 0 {
		return ""
	}
	var (
		sec, k string
		secKey = strings.Split(key, "::")
	)

	if len(secKey) >= 2 {
		sec = secKey[0]
		k = secKey[1]
	} else {
		sec = defaultSection
		k = secKey[0]
	}
	if v, ok := this.data[sec]; ok {
		if vv, ok := v[k]; ok {
			return vv
		}
	}
	return ""
}

func (this *Configer) SetBool(key string, value bool) {
	this.set(key, strconv.FormatBool(value))
}
func (this *Configer) SetInt(key string, value int) {
	this.set(key, strconv.FormatInt(int64(value), 32))
}

func (this *Configer) SetInt64(key string, value int64) {
	this.set(key, strconv.FormatInt(int64(value), 64))
}
func (this *Configer) SetFloat32(key string, value float32) {
	this.set(key, strconv.FormatFloat(float64(value), 'E', -1, 32))
}
func (this *Configer) SetFloat64(key string, value float64) {
	this.set(key, strconv.FormatFloat(float64(value), 'E', -1, 64))
}
func (this *Configer) SetString(key string, value string) {
	this.set(key, value)
}
func (this *Configer) SetStrings(key string, values []string) {
	this.set(key, strings.Join(values, ","))
}

func (this *Configer) GetBool(key string) bool {
	return this.TryGetBool(key, false)
}
func (this *Configer) GetInt(key string) int {
	return this.TryGetInt(key, 0)
}

func (this *Configer) GetInt64(key string) int64 {
	return this.TryGetInt64(key, 0)
}
func (this *Configer) GetFloat32(key string) float32 {
	return this.TryGetFloat32(key, 0)
}
func (this *Configer) GetFloat64(key string) float64 {
	return this.TryGetFloat64(key, 0)
}
func (this *Configer) GetString(key string) string {
	return this.TryGetString(key, "")
}
func (this *Configer) GetStrings(key string) []string {
	return this.TryGetStrings(key, []string{})
}

func (this *Configer) TryGetBool(key string, defaultValue bool) bool {
	s := this.get(key)
	if s == "" {
		return defaultValue
	}
	r, e := strconv.ParseBool(s)
	if e != nil {
		return defaultValue
	}
	return r
}
func (this *Configer) TryGetInt(key string, defaultValue int) int {
	s := this.get(key)
	if s == "" {
		return defaultValue
	}
	r, e := strconv.ParseInt(s, 10, 32)
	if e != nil {
		return defaultValue
	}
	return int(r)
}

func (this *Configer) TryGetInt64(key string, defaultValue int64) int64 {
	s := this.get(key)
	if s == "" {
		return defaultValue
	}
	r, e := strconv.ParseInt(s, 10, 64)
	if e != nil {
		return defaultValue
	}
	return r
}
func (this *Configer) TryGetFloat32(key string, defaultValue float32) float32 {
	s := this.get(key)
	if s == "" {
		return defaultValue
	}
	r, e := strconv.ParseFloat(s, 32)
	if e != nil {
		return defaultValue
	}
	return float32(r)
}
func (this *Configer) TryGetFloat64(key string, defaultValue float64) float64 {
	s := this.get(key)
	if s == "" {
		return defaultValue
	}
	r, e := strconv.ParseFloat(s, 64)
	if e != nil {
		return defaultValue
	}
	return float64(r)
}
func (this *Configer) TryGetString(key string, defaultValue string) string {
	s := this.get(key)
	if s == "" {
		return defaultValue
	}
	return s
}
func (this *Configer) TryGetStrings(key string, defaultValues []string) []string {
	s := this.get(key)
	if s == "" {
		return defaultValues
	}
	return strings.Split(s, ",")
}

func (this *Configer) ParseFile(fileName string) error {
	this.Lock()
	defer this.Unlock()
	err := this.parseFile(fileName)
	return err
}

func (this *Configer) ParseString(s string) error {
	this.Lock()
	defer this.Unlock()
	err := this.parseString(s)
	return err
}

func NewConfigerFile(fileName string) (*Configer, error) {
	r := &Configer{
		sync.RWMutex{},
		[]string{},
		make(map[string]map[string]string),
	}
	e := r.ParseFile(fileName)
	return r, e
}

func NewConfigerString(s string) (*Configer, error) {
	r := &Configer{
		sync.RWMutex{},
		[]string{},
		make(map[string]map[string]string),
	}
	e := r.ParseString(s)
	return r, e
}
