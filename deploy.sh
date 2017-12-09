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
    $(aws ecr get-login --region $region --no-include-email) # does docker login
    set -x

    echo "# Pushing Docker Image"
    docker tag $IMAGE_NAME:latest $REPO_URL/$service:$TRAVIS_BUILD_NUMBER
    docker push $REPO_URL/$service:$TRAVIS_BUILD_NUMBER

    set +x

    echo "# Finished pushing docker images"
}

update_task_definition()
{
    echo "####### Update task definition and running blue/green on tasks"

	npm install aws-sdk-promise
	npm install aws-sdk
	npm install chalk
	npm install commander

    node ecs-task-deploy.js \
        -k $AWS_ACCESS_KEY_ID \
        -s $AWS_SECRET_ACCESS_KEY \
        -r $region \
        -c $cluster \
        -n $service \
        -i $REPO_URL/$service:$TRAVIS_BUILD_NUMBER \
        -e APPLICATION_VERSION=$TRAVIS_BUILD_NUMBER \
        -t 300
        -v
}

truncate_images_in_dc()
{
    echo "# Truncating docker images in ECR (will keep the latest 5)"
    bash `pwd`/ecr-truncate_docker_images --repository $service --region $region
    echo "# Finished truncating docker images"
}



# Run the deployment
eval_aws_variables 
push_image_to_dc 
update_task_definition 
truncate_images_in_dc 



exit 0