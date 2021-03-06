# Lightsail Container Setup

## Create container service

1. Select the correct service location, as certain AWS service features are geo-locked

![image](https://user-images.githubusercontent.com/4997221/166152670-c00d0f58-238d-4ed8-b830-021aa7467426.png)

---

2. Select the desired power and scale

![image](https://user-images.githubusercontent.com/4997221/166152526-7a653aac-155f-47f1-91a0-acdb0774c042.png)

---

3. Once the container is provisioned, upload a local container image using AWS CLI. Details can be found [here](https://lightsail.aws.amazon.com/ls/docs/en_us/articles/amazon-lightsail-pushing-container-images)

---

4. Create a deployment

![image](https://user-images.githubusercontent.com/4997221/166153760-14bbd49c-13f4-44af-a0c7-98f067d1f81a.png)

Save and deploy

![image](https://user-images.githubusercontent.com/4997221/166153872-93cdef7c-29be-4cb6-b750-ea859f6aa112.png)

We can automate set 3 and 4 as part of our CI pipeline, an example can be found [here](https://github.com/adelinosousa/huh.sh/blob/main/CI/Build%20Push%20Deploy.yml)

## Custom domain setup

1. DNS creation and domain name server update

We'll need to setup a DNS Zone to validate our SSL certificate. On the lightsail dashboard find the network tab and select "Create DNS Zone"

![image](https://user-images.githubusercontent.com/4997221/166153992-2d02b311-9018-466e-b5f3-881ebd3300ac.png)

Enter the domain you've registered and create the DNS Zone

![image](https://user-images.githubusercontent.com/4997221/166154088-91a10372-681b-415a-b570-e152936fe2ec.png)

After creation, you'll have a set of name servers

![image](https://user-images.githubusercontent.com/4997221/166154334-9898f178-aa46-4fe6-9041-0f69b2166ff0.png)

You'll need to update the name servers where your domain is registered. If you used Route 53 to register your domain you can update it by visiting the Route 53 registered domain and editing the name servers

![image](https://user-images.githubusercontent.com/4997221/166154475-30684242-9326-4a9a-8c47-e5e42bddad72.png)

This may take a while, you should receive an email confirmation once the update is complete

---

2. Generating SSL certificate and DNS hostname setup

Select your container and find the "Custom Domains" tab

![image](https://user-images.githubusercontent.com/4997221/166154763-ef636949-982e-46d7-b8bc-4df9b07fbaae.png)

Create SSL certificate

![image](https://user-images.githubusercontent.com/4997221/166154837-c63e8637-e82b-4b78-a40e-7f65cdfe6862.png)

To validate certificate, navigate to the DNS Zone and add CNAME and the A record

![image](https://user-images.githubusercontent.com/4997221/166154993-4ff6bed4-b9cb-43a2-bedf-b022dd61b047.png)
![image](https://user-images.githubusercontent.com/4997221/166155352-b12e8f67-c0c6-4c75-8bfa-a1399a193de1.png)

*Note*: "A" record must resolve to your container using "@" as the subdomain, example

![image](https://user-images.githubusercontent.com/4997221/166155258-6c0d9ff2-6c5b-429c-b057-ac68d3f439a1.png)

Once the SSL certificate is validated, attach it to the container service in the "Custom Domains" tab

## Email setup with custom domain (SES)

Create identity (domain)

![image](https://user-images.githubusercontent.com/4997221/166155796-eb5621c1-628d-44d8-992a-2dfa2984df6b.png)

Verify identity (domain) by adding the CNAME records to the DNS zone

![image](https://user-images.githubusercontent.com/4997221/166156273-c8f3418e-7ca4-41c8-9c61-6b01db383a7e.png)

Add DNS MX record to direct email to the mail server. Skip this step if you dont need to receive emails

Subdomain: "@" 
Maps to: "inbound-smtp.<aws region>.amazonaws.com"





