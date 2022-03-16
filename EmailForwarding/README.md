# AWS SES Email Forwarding

Forwards received SES emails 📧. Example support@business.com >> personal@email.com.

## Setup
- Use cloudformation [script](https://github.com/adelinosousa/demos/blob/main/EmailForwarding/index.json) to create necessary resources and policies
- Modify [lambda](https://github.com/arithmetric/aws-lambda-ses-forwarder/blob/master/index.js) code accordingly and upload it to *ef-ses* bucket as *ef-function-emailreceived.zip*
- Activate *ef-ses-rules* SES email receiving rule. To do this: 
  - Log in to AWS console
  - Navigate to SES
  - Select *Email Receiving*. Not all AWS regions have email receiving feature. For example *eu-west-1* does but *eu-west-2* does not.
  - Select *ef-ses-rules* rule and set it as as active
