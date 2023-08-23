# DEF-PIPE Frontend

This project is a continuation of the work based on https://github.com/DataCloud-project/DEF-PIPE-Frontend.

The objective of this project is to add a functionality that transforms defined Big Data pipelines into YAML files, which can be executed by Argo Workflow. Additionally, it aims to increase the complexity of the pipelines.

The application is now deployed on GKE(Google Kubernetes Engine), and can be accessed at http://34.88.4.162:80.

## Completed Tasks

1. Implemented the export of all pipelines, including pipelines with multiple inputs/outputs and subpipelines.
2. Added the export of parameters such as environment variables and container images in the generated YAML file.
3. Design and implement support for loops and conditionals within the pipelines.

## Remaining Tasks

1. Include the export of additional parameters such as execution requirements and resource providers.
