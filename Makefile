.PHONY: ALL

image:
	docker build -t vivasaayi/selenium-with-nodejs .
test-runner:
	docker build -f Dockerfile_Runner -t vivasaayi/test-runner .
run-tr:
	docker run -it -v /var/run/:/var/run/ -e ALLOWEDCONTAINERS=5 vivasaayi/test-runner
run-test:
	docker run --rm -d -v /dev/shm:/dev/shm -e NOVNC=true vivasaayi/selenium-with-nodejs:latest