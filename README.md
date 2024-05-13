%3|1715596577.054|FAIL|rdkafka#producer-1| [thrd:sasl_ssl://kaftestn3.halykbank.nb:9093/bootstrap]: sasl_ssl://kaftestn3.halykbank.nb:9093/bootstrap: SSL handshake failed: error:0A000086:SSL routines::certificate verify failed: broker certificate could not be verified, verify that ssl.ca.location is correctly configured or root CA certificates are installed (install ca-certificates package) (after 7ms in state SSL_HANDSHAKE, 11 identical error(s) suppressed)
%3|1715596577.054|ERROR|rdkafka#producer-1| [thrd:app]: rdkafka#producer-1: sasl_ssl://kaftestn3.halykbank.nb:9093/bootstrap: SSL handshake failed: error:0A000086:SSL routines::certificate verify failed: broker certificate could not be verified, verify that ssl.ca.location is correctly configured or root CA certificates are installed (install ca-certificates package) (after 7ms in state SSL_HANDSHAKE, 11 identical error(s) suppressed) 

Посмотри, что это за ошибка, локально работает, но на сервере нет. 

        public LCOService(IOptions<AppSettings> options, KafkaProducer producer)
        {
            _options = options;
            _produverConfig = new ProducerConfig()
            {
                BootstrapServers = options.Value.KafkaSettings.BootstrapServers,
                Acks = Acks.Leader,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = options.Value.KafkaSettings.SaslUsername,
                SaslPassword = options.Value.KafkaSettings.SaslPassword
            };
        }

        public async Task ProduceStatus(string message) 
        {
            await KafkaProducer.ProduceAsync(_produverConfig, _options.Value.KafkaSettings.Topic, message);
        }
