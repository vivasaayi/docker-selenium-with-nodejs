FROM elgalu/selenium

RUN curl -sL https://deb.nodesource.com/setup_8.x | sudo bash -

RUN sudo apt-get -y install build-essential
RUN sudo apt -y install cpio
RUN sudo apt -y install nodejs

RUN node -v
RUn npm -v

USER root

RUN mkdir -p /app_root
ENV HOME=/app_root

WORKDIR /app_root

COPY . .

RUN npm install

USER seluser
WORKDIR /home/seluser

COPY start-test.sh .

CMD ["sh", "start-test.sh"]