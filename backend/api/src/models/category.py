import json
from models.shared import IRequest, ParseResult, Validation

class CreateCategoryRequest(IRequest):
    def __init__(self, label, suggested_tags):
        self.label = label
        self.suggested_tags = suggested_tags

    def validate(self):
        errors = []
        if len(self.label) == 0 or len(self.label) > 100:
            errors.append("Length of label must be between 0 and 100")
        return Validation(errors=errors)

    @staticmethod
    def parse(event):
        errors = []
        if "body" not in event:
            errors.append("Body is required")
            return ParseResult(result=None, errors=errors)

        body = json.loads(event["body"])
        if "label" not in body:
            errors.append("Label is required")
            return ParseResult(result=None, errors=errors)

        if "suggested_tags" not in body:
            body["suggested_tags"] = []

        try:
            result = CreateCategoryRequest(**body)
            return ParseResult(result=result, errors=errors)
        except Exception as exception:
            errors.append("Could not parse request body")
            return ParseResult(result=None, errors=errors)
