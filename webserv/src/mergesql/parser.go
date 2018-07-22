package main

import (
	"fmt"
	"io/ioutil"
	"os"
	"path/filepath"
	"strings"
)

const (
	defaultSection = "default"
	includePrefix  = "include"
)

func (this *Configer) prepareFileName(fileName string) (string, error) {
	if !filepath.IsAbs(fileName) {
		if l := len(this.files); l == 0 {
			fileName, _ = filepath.Abs(fileName)
		} else {
			fileName = filepath.Dir(this.files[l-1]) + "/" + fileName
			fileName, _ = filepath.Abs(fileName)
		}
	}
	fileName = filepath.FromSlash(fileName)
	for _, p := range this.files {
		if p == fileName {
			return "", fmt.Errorf("cant not include file(%s) tiwce", fileName)
		}
	}
	this.files = append(this.files, fileName)
	return fileName, nil
}

//转换替换环境变量
func (this *Configer) replaceEnv(s string) string {
	//${ENV}/&{ENV}.${ENV}
	r := ""
	for {
		i := strings.Index(s, "${")
		if i < 0 {
			r += s
			return r
		}
		r += s[:i]
		s = s[i+2:]
		i = strings.Index(s, "}")
		if i < 0 {
			r += s
			return r
		}
		k := s[:i]
		s = s[i+1:]
		p := os.Getenv(k)
		if len(p) == 0 {
			r += fmt.Sprintf("[%s|Not found!]", k)
		} else {
			r += p
		}
	}
}

func (this *Configer) parseFile(fileName string) error {

	f, e := this.prepareFileName(fileName)

	if e != nil {
		return e
	}

	s, e := ioutil.ReadFile(f)
	if e != nil {
		return e
	}

	return this.parseString(string(s))
}

func (this *Configer) parseString(s string) error {
	sec := defaultSection
	ss := strings.Split(s, "\n")
	for i, s := range ss {
		s = strings.Trim(s, "\r\n ")
		if len(s) == 0 { //空行
			continue
		}
		if s[0] == '#' || s[0] == ';' { //注释
			continue
		}
		if strings.HasPrefix(s, includePrefix) { //导入
			inc := strings.Fields(s)
			if len(inc) < 2 {
				return fmt.Errorf("include none file at line %d", i)
			}
			inc = inc[1:] //去掉include 标记
			for _, f := range inc {
				f = strings.Trim(f, "\r\n\"' ") //去掉无效符号
				if len(f) == 0 {
					continue
				}
				e := this.parseFile(f)
				if e != nil {
					return e
				}
			}
			continue
		}
		if s[0] == '[' { //section
			if s[len(s)-1] != ']' {
				return fmt.Errorf("parse ini config section mask is not match at line %d", i)
			}
			s = strings.Trim(s, "[]\r\n ")
			s = strings.ToLower(s)
			if len(s) == 0 { // end section change to default section
				sec = defaultSection
			} else {
				sec = s
			}
			continue
		}

		ei := strings.IndexRune(s, '=')
		if ei > 0 { //有赋值符号
			k, v := strings.ToLower(s[:ei]), s[ei+1:]
			v = strings.Trim(v, "\r\n\" ")
			//转换环境变量
			if strings.Contains(v, "${") {
				v = this.replaceEnv(v)
			}

			if _, ok := this.data[sec]; !ok {
				this.data[sec] = make(map[string]string)
			}
			this.data[sec][k] = v
		}
	}

	return nil
}
