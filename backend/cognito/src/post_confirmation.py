import json
import logging
import boto3
import os

logger = logging.getLogger()
logger.setLevel(logging.INFO)

cognito = boto3.client('cognito-idp')
dynamodb = boto3.resource('dynamodb').Table(os.environ.get("TABLE"))

def lambda_handler(event, context):
    """
    Post confirmation lambda trigger
    Adds the newly confirmed user to DynamoDB 
    & adds them the the 'Users' group
    """
    logger.info("Post confirmation trigger running.")
    logger.info("Recieved event: " + json.dumps(event))

    try:
        username = event["userName"]

        user_id = f'USER#{username}'
        data = {
            'PK': user_id,
            'SK': user_id
        }

        logger.info('Inserting user: ' + json.dumps(data))

        dynamodb.put_item(Item=data)
        logger.info('Inserted data: ' + json.dumps(data))
        
    except Exception as exception:
        logger.error('Error inserting user: ' + str(exception))

    try:
        logger.info("Adding user to Users group")
        cognito.admin_add_user_to_group(UserPoolId=event["userPoolId"], Username=username, GroupName="Users")
    except Exception as exception:
        logger.error("Error adding user to users group: " + str(exception))

    logger.info("Returning event: " + json.dumps(event))
    
    return event
