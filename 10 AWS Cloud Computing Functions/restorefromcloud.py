import os
import boto3
import base64
import argparse

# ------------------------------
# 
#
# restorefromcloud.py
#
# Downloads files from s3 bucket to a directory 
#
#
# ------------------------------ 

## Parse in and handle arguments 
parser = argparse.ArgumentParser()
parser.add_argument("-i", "--initialize", help="flag used to create a s3 bucket",  action='store_true')
args = parser.parse_args()


ROOT_DIR = './test_download/'


#local variables 
bucker_name = "21484971-cloudstorage"; 
availability_zone= "ap-southeast-2"
bucket_config = {'LocationConstraint': 'ap-southeast-2'}

def download_files():
    """Downloads files to a target directory from an S3 bucket

    """
    ## Arrays for keys and directories 
    keys = []
    dirs = []

    ## Get the contents of the s3 bucket where name is equal to input name 
    contents=s3_client.list_objects(Bucket=bucker_name)['Contents']

    ##print this info for debugging 
    print(contents)
    
    ## loop through the contents items
    for s3_obj in contents:

        ## Get the key of the response object
        s3_key = s3_obj['Key']
        
        ## Check the end of the string to see if directory of key 
        if s3_key[-1] != '/':
            keys.append(s3_key)
        else:
            dirs.append(s3_key)
                
        ## Loop through and create all required directories to ensure the paths exist 
        for d in dirs:
            dest_pathname = os.path.join(ROOT_DIR, d)
            if not os.path.exists(os.path.dirname(dest_pathname)):
                os.makedirs(os.path.dirname(dest_pathname))

        ## Loop through each key (s3 object) and download to the path
        for k in keys:
            dest_pathname = os.path.join(ROOT_DIR, k)
            if not os.path.exists(os.path.dirname(dest_pathname)):
                os.makedirs(os.path.dirname(dest_pathname))
            
            ## Download items from s3
            s3_client.download_file(bucker_name, k, dest_pathname)


# Main program
# Insert code to create bucket if not there
try:
    print("start")

    ## Connect to s3
    s3 = boto3.resource('s3', availability_zone)
    s3_client = boto3.client("s3", region_name=availability_zone) 
    print("Connected to s3")

    ## Retrieve the list of existing buckets
    response = s3_client.list_buckets()

    ## Set a found variable before searching for bucket
    found_bucket = False

    ## check for bucket with bucket name 
    for bucket in response['Buckets']:
        name =  bucket["Name"]
        if(name == bucker_name):
            print("Bucket exists and has been found.")
            print(bucket)
            download_files()
            found_bucket = True

    ## If cannot find bucket, print to the user error 
    if(found_bucket == False):
        print("Could not find your bucket")
    
## Handle any exceptions 
except Exception as error:
    print(error)
    pass

print("done")