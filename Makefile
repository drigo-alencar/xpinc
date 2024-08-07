NETWORK_NAME = xpinc
API_CONTAINER_NAME = api

all: start

ensure-network:
ifeq ($(OS),Windows_NT)
	@podman network ls --filter name=$(NETWORK_NAME) --format "{{ .Name }}" | findstr /R "^$(NETWORK_NAME)" >nul || podman network create $(NETWORK_NAME)
else
	@if ! podman network ls --filter name=$(NETWORK_NAME) --format '{{ .Name }}' | grep -q $(NETWORK_NAME); then \
		podman network create $(NETWORK_NAME); \
	fi
endif

start: ensure-network
	podman run --name db --network $(NETWORK_NAME) --network-alias db -d -p 27017:27017 docker.io/mongodb/mongodb-community-server
	podman build -t portfolio -f ./portfolio/Dockerfile .
	podman run --name api -d -p 5000:5000 --network $(NETWORK_NAME) --network-alias api portfolio:test


clean:
ifeq ($(OS),Windows_NT)
	podman stop $(API_CONTAINER_NAME) || exit /b 0
	podman rm $(API_CONTAINER_NAME) || exit /b 0
	podman stop db || exit /b 0
	podman rm db || exit /b 0
else
	podman stop $(API_CONTAINER_NAME) || true
	podman rm $(API_CONTAINER_NAME) || true
	podman stop db || true
	podman rm db || true
endif
