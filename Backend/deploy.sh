#!/bin/sh
GIT_VERSION=$(git describe --tags)
docker build -t registry.aff.ng/TweyesBackend_core:$GIT_VERSION -f TweyesBackend/Dockerfile .
docker build -t registry.aff.ng/TweyesBackend_admin:$GIT_VERSION -f TweyesBackend/Dockerfile .
docker push registry.aff.ng/TweyesBackend_core:$GIT_VERSION
docker push registry.aff.ng/TweyesBackend_admin:$GIT_VERSION

