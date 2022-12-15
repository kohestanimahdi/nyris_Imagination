# Feel free to replace this with a more appropriate Dockerfile.

FROM alpine
RUN apk update
CMD ["apk", "fetch", "coffee"]
