#!/bin/bash

eval_aws_variables()
{

	eval REPO_URL=\$$REPO_URL
	eval AWS_ACCESS_KEY_ID=\$$AWS_ACCESS_KEY_ID
	eval AWS_SECRET_ACCESS_KEY=\$$AWS_SECRET_ACCESS_KEY

    # Validate the environment variables exist
    : ${REPO_URL?"Need to set environment variable for $REPO_URL"}
    : ${AWS_ACCESS_KEY_ID?"Need to set environment variable for $AWS_ACCESS_KEY_ID"}
    : ${AWS_SECRET_ACCESS_KEY?"Need to set environment variable for $AWS_SECRET_ACCESS_KEY"}
    : ${TRAVIS_BUILD_NUMBER?"Need to set environment variable for TRAVIS_BUILD_NUMBER"}

    export AWS_ACCESS_KEY_ID=$AWS_ACCESS_KEY_ID
    export AWS_SECRET_ACCESS_KEY=$AWS_SECRET_ACCESS_KEY
    export REPO_URL=$REPO_URL
}

push_image_to_dc()
{
    echo "# Pushing docker images to $REPO_URL"
    $(aws ecr get-login --region ap-southeast-2 --no-include-email) # does docker login
    set -x

    echo "# Pushing Docker Image"
    docker tag $IMAGE_NAME:latest $REPO_URL/$service:$TRAVIS_BUILD_NUMBER
    docker push $REPO_URL/$service:$TRAVIS_BUILD_NUMBER

    set +x

    echo "# Finished pushing docker images"
}




# Run the deployment
eval_aws_variables 
push_image_to_dc 




exit 0