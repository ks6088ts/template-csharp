# Git
GIT_REVISION ?= $(shell git rev-parse --short HEAD)
GIT_TAG ?= $(shell git describe --tags --abbrev=0 --always | sed -e s/v//g)

# Docker
DOCKER_REPO_NAME ?= ks6088ts
DOCKER_IMAGE_NAME ?= template-csharp
DOCKER_COMMAND ?=

# Tools
TOOLS_DIR ?= /usr/local/bin
# https://github.com/aquasecurity/trivy/releases
TRIVY_VERSION ?= 0.69.3

# Misc
OUTPUT_DIR ?= obj
CLI_PROJECT ?= src/TemplateCsharp.Cli/TemplateCsharp.Cli.csproj

.PHONY: help
help:
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'
.DEFAULT_GOAL := help

.PHONY: install-deps-dev
install-deps-dev: ## install dependencies for development
	@# https://aquasecurity.github.io/trivy/v0.18.3/installation/#install-script
	@which trivy || curl -sfL https://raw.githubusercontent.com/aquasecurity/trivy/main/contrib/install.sh | sh -s -- -b $(TOOLS_DIR) v$(TRIVY_VERSION)
	@which actionlint || echo "install actionlint https://github.com/rhysd/actionlint"
	dotnet restore --verbosity detailed

.PHONY: format-check
format-check: ## check code formatting without making changes
	dotnet format --verify-no-changes --verbosity detailed

.PHONY: format
format: ## format code
	dotnet format --verbosity detailed

.PHONY: lint
lint: ## lint GitHub Actions workflows and C# code
	actionlint
	dotnet build --verbosity detailed -warnaserror

.PHONY: test
test: ## run tests
	dotnet test --verbosity detailed

.PHONY: build
build: ## build applications
	dotnet build --verbosity detailed

.PHONY: ci-test
ci-test: install-deps-dev format-check lint test build ## run CI tests

.PHONY: update
update: ## update NuGet packages
	dotnet outdated --verbosity detailed || true

.PHONY: release
release: ## publish CLI application
	dotnet publish $(CLI_PROJECT) -c Release -o $(OUTPUT_DIR) --verbosity detailed

.PHONY: run
run: ## run CLI application
	dotnet run --project $(CLI_PROJECT) -- --verbose

# ---
# Docker
# ---

.PHONY: docker-build
docker-build: ## build Docker image
	docker build \
		-t $(DOCKER_REPO_NAME)/$(DOCKER_IMAGE_NAME):$(GIT_TAG) \
		--build-arg GIT_REVISION=$(GIT_REVISION) \
		--build-arg GIT_TAG=$(GIT_TAG) \
		.

.PHONY: docker-run
docker-run: ## run Docker container
	docker run --rm $(DOCKER_REPO_NAME)/$(DOCKER_IMAGE_NAME):$(GIT_TAG) $(DOCKER_COMMAND)

.PHONY: docker-lint
docker-lint: ## lint Dockerfile
	docker run --rm -i hadolint/hadolint < Dockerfile

.PHONY: docker-scan
docker-scan: ## scan Docker image
	trivy image $(DOCKER_REPO_NAME)/$(DOCKER_IMAGE_NAME):$(GIT_TAG)

.PHONY: ci-test-docker
ci-test-docker: install-deps-dev docker-lint docker-build docker-scan docker-run ## run CI tests for Docker
