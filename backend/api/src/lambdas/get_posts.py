import json
import math
from data.category import CategoryRepository
from utils import get_dynamodb, get_logger, get_time, remove_prefix, lambda_response 
from boto3.dynamodb.conditions import Key

logger, dynamodb = get_logger(), get_dynamodb()

def lambda_handler(event, context):    
    if "queryStringParameters" not in event:
        return lambda_response(status_code=400, headers={}, body={ "errors": "Search paramaters required" })
    
    query_parameters = event["queryStringParameters"]
    if "category" not in query_parameters:
        return lambda_response(status_code=400, headers={}, body={ "errors": "Category required" })
    
    category = query_parameters['category']
    start = query_parameters["start"] if "start" in query_parameters else get_time()
    limit = min(100, int(query_parameters["limit"])) if "limit" in query_parameters else 10
    tags = query_parameters["tags"].split(",") if "tags" in query_parameters else []

    kce = Key("GSI3PK").eq(f"{CategoryRepository.prefix}#{category}") 
    if "end" in query_parameters:
        kce = kce & Key("GSI3SK").between(query_parameters["end"], start)
    else:    
        kce = kce & Key("GSI3SK").lt(start)

    results = dynamodb.query(
        IndexName="GSI3",
        KeyConditionExpression=kce,
        ScanIndexForward = False,
        Limit = limit
    )["Items"]

    posts = []
    for result in results:
        attributes = json.loads(result["attributes"])

        tag_intersection = list(set(tags) & set(attributes["tags"]))
        if len(tags) > 0 and len(tag_intersection) == 0:
            continue

        post = {
            "post_id": remove_prefix(result["PK"]),
            "category": remove_prefix(result["GSI3PK"]),
            "created_at": math.floor(float(result["GSI3SK"])),
            "description": attributes["description"],
            "title": attributes["title"],
            "tags": attributes["tags"]
        }
        posts.append(post)

    return lambda_response(status_code=200, headers={}, body={ "posts": posts })
