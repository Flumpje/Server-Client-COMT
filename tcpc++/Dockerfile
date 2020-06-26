FROM amytabb/docker_ubuntu16_essentials

COPY . /server/
WORKDIR /server/
RUN g++ server.cpp -o server ./libws2_32.a
CMD ["./server"]

