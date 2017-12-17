#!/bin/bash

aws ecr get-login --no-include-email --region ap-southeast-2
docker build -t personal.brycefamily .
docker tag personal.brycefamily:latest 896879620219.dkr.ecr.ap-southeast-2.amazonaws.com/personal.brycefamily:latest
docker push 896879620219.dkr.ecr.ap-southeast-2.amazonaws.com/personal.brycefamily:latest