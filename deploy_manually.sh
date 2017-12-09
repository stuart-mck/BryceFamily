#!/bin/bash

# You can get these out of the KOI_Passwords_v1 KeePass file
export DC0_REPO_URL=""
export DC0_AWS_ACCESS_KEY_ID=""
export DC0_AWS_SECRET_ACCESS_KEY=""
export TRAVIS_BUILD_NUMBER=""
export TRAVIS_BRANCH="master"

unset AWS_SECRET_ACCESS_KEY
unset AWS_LAST_UPDATE
unset AWS_SESSION_TOKEN
unset AWS_ACCESS_KEY_ID

echo -e "\e[32mHINT: If after confirming the above keys are correct you still get a (AccessDeniedException) from AWS then it's probably because the Travis_Platform account has the IAM_Travis policy attached which prevents it from being used in the office."
echo -e -n '\e[0;0m'

echo -e -n "\e[31mAre you sure you want to do this? It will push your local machine to ALL environments!!! [y/N] "
echo -e -n '\e[0;0m'
read response

if [[ "$response" =~ ^([yY][eE][sS]|[yY])+$ ]]
then
	docker-compose build
	./deploy.sh
fi