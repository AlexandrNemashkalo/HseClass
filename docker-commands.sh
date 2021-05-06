sudo docker build -t api -f HseClass.Api/Dockerfile .
sudo docker-compose -f "docker-compose.yml" up -d