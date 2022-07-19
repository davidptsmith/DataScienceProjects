import os
import boto3
import base64
import argparse


# ------------------------------
# 
#
# cloudstorage.py
#
# skeleton application to copy local files to S3
#
# Given a root local directory, will return files in each level and
# copy to same path on S3
#
# ------------------------------ 

## Handle arguments 
parser = argparse.ArgumentParser()
parser.add_argument("-i", "--initialize", help="flag used to create a s3 bucket",  action='store_true')
args = parser.parse_args()


ROOT_DIR = '.'
ROOT_S3_DIR = '21484971-cloudstorage'

#local variables 
bucker_name = "21484971-cloudstorage"; 
availability_zone= "ap-southeast-2"
bucket_config = {'LocationConstraint': 'ap-southeast-2'}


def create_bucket():
    """Creates a S3 bucket on AWS

    """
     
     #use try catch to check if bucket already exists (not the best way but eh, it works.)
    try:
        #connect bucket 
        response = s3.create_bucket(Bucket=bucker_name, CreateBucketConfiguration=bucket_config)
        print("Created Bucket, Response:")
        print(response)

    except Exception as error:
        print("Bucket may already exists.\nError Message:\n" )
        print(error)

def getFilesAndUploadToS3():
    """Steps through the directory and uploads the files to S3
    """
    for dir_name, subdir_list, file_list in os.walk(ROOT_DIR, topdown=True):
        if dir_name != ROOT_DIR:
            for fname in file_list:
                upload_file("%s/" % dir_name[2:], "%s/%s" % (dir_name, fname), fname)
    print("Files Uploaded")

def upload_file(folder_name, file, file_name):
    """Uploads files to the S3 buckets
    """
    ## Upload the file with the object path & key
    try:
        s3_client.upload_file(file, bucker_name, folder_name + file_name)
    except ClientError as e:
        print(e)




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
            found_bucket = True

    ## if not found and -i flag present, create bucket 
    if( found_bucket == False & args.initialize):
        print("Bucket not found: Creating One")
        create_bucket()
        found_bucket = True

    ## upload files if bucket exists
    if(found_bucket):
        getFilesAndUploadToS3()
    elif  ( found_bucket == False & args.initialize != True):
        print("Could not find a bucket to upload to, please use the -i flag and try again")

except Exception as error:
    print(error)
    pass

print("done")