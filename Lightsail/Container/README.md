# Lightsail Container Setup

## Create container service

1. Select the correct service location, as certain AWS service features are geo-locked

![image](https://user-images.githubusercontent.com/4997221/166152670-c00d0f58-238d-4ed8-b830-021aa7467426.png)

2. Select the desired power and scale

![image](https://user-images.githubusercontent.com/4997221/166152526-7a653aac-155f-47f1-91a0-acdb0774c042.png)

3. Once the container is provisioned, upload a local container image using AWS CLI. Details can be found [here](https://lightsail.aws.amazon.com/ls/docs/en_us/articles/amazon-lightsail-pushing-container-images)

4. Create a deployment

![image](https://user-images.githubusercontent.com/4997221/166153760-14bbd49c-13f4-44af-a0c7-98f067d1f81a.png)

Save and deploy

![image](https://user-images.githubusercontent.com/4997221/166153872-93cdef7c-29be-4cb6-b750-ea859f6aa112.png)

We can automate set 3 and 4 as part of our CI pipeline, an example can be found [here](https://github.com/adelinosousa/huh.sh/blob/main/CI/Build%20Push%20Deploy.yml)

## Custom domain setup

We'll need to setup a DNS Zone to validate our SSL certificate. On the lightsail dashboard find the network tab and select "Create DNS Zone"

![image](https://user-images.githubusercontent.com/4997221/166153992-2d02b311-9018-466e-b5f3-881ebd3300ac.png)








