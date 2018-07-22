package models

import (
	"encoding/csv"
	"errors"
	"io"
	"io/ioutil"
	"strconv"
	"strings"
)

type Csvf struct {
	Names   []string
	Records [][]string
}

func (c *Csvf) Load(fn string) error {
	bytes, err := ioutil.ReadFile(fn)

	if err != nil {
		return err
	}
	csvr := csv.NewReader(strings.NewReader(string(bytes)))

	c.Names, err = csvr.Read()

	for {
		record, err := csvr.Read()
		if err == io.EOF {
			break
		}
		if err != nil {
			return err
		}

		if len(c.Names) != len(record) {
			return errors.New("record length not match")
		}
		c.Records = append(c.Records, record)
	}

	return nil
}

func (c *Csvf) get_string(row int, col int) (string, error) {
	if row < 0 && row >= len(c.Records) {
		return "", errors.New("row outside")
	}
	if col < 0 && col >= len(c.Names) {
		return "", errors.New("col outside")
	}
	return c.Records[row][col], nil
}

func (c *Csvf) get_column_index(name string) (int, error) {
	alen := len(c.Names)
	var index int = 0
	for {
		if index >= alen {
			return -1, errors.New("Can not find column")
		}
		if c.Names[index] == name {
			return index, nil
		}
		index++
	}
}

func (c *Csvf) GetString(row int, colname string) (string, error) {
	col, err := c.get_column_index(colname)
	if err != nil {
		return "", err
	}
	str, err := c.get_string(row, col)
	if err != nil {
		return "", err
	}
	return str, nil
}

func (c *Csvf) GetInt(row int, colname string) (int, error) {
	str, err := c.GetString(row, colname)
	if err != nil {
		return 0, err
	}
	return strconv.Atoi(str)
}

func (c *Csvf) GetRecordLength() int {
	return len(c.Records)
}

func (c *Csvf) GetColumnLength() int {
	return len(c.Names)
}

func NewCsvf() *Csvf {
	return &Csvf{}
}

func LoadCsvf(fn string) (*Csvf, error) {
	csvf := NewCsvf()
	err := csvf.Load(fn)
	if err != nil {
		return nil, err
	}
	return csvf, nil
}
