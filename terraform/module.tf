provider "aws" {
  region = "ap-southeast-1"
  access_key = ""
  secret_key = ""
}

# VPC
resource "aws_default_vpc" "default" {

}

# Security Group for Backend
resource "aws_security_group" "cdn-sg-backend" {
  name        = "cdn-sg-backend"
  description = "Allow incoming connections"
  vpc_id      = aws_default_vpc.default.id
  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow incoming HTTPS connections"
  }
  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow incoming SSH connections"
  }
  ingress {
    from_port   = 5432
    to_port     = 5432
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow incoming postgres connections"
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = {
    Name = "cdn-sg-backend"
  }
}

# AMI Data Source
data "aws_ami" "cdn-al-image" {
  most_recent = true
  owners = ["amazon"]

  filter {
      name   = "name"
      values = ["al2023-ami-2023*"]
  }

  filter {
    name   = "architecture"
    values = ["x86_64"]
  }
}

resource "aws_key_pair" "cs_key_pair" {
  key_name   = "cdn-key-pair"
  public_key = file("/Users/akar/.ssh/id_rsa_ec2.pub")  # Path to your public key file
}


# EC2 Instance for Backend
resource "aws_instance" "cdn-vm-backend" {
  ami                    = data.aws_ami.cdn-al-image.id
  instance_type          = "t2.small"
  vpc_security_group_ids = [aws_security_group.cdn-sg-backend.id]
  associate_public_ip_address = true
  key_name               = aws_key_pair.cs_key_pair.key_name

  tags = {
    Name = "cdn-vm-backend"
  }

  # Install Node.js, npm, and run NestJS on instance creation
  user_data = <<-EOF
#!/bin/bash
sudo dnf update -y
sudo dnf install -y docker
sudo usermod -a -G docker ec2-user 
sudo dnf install -y certbot python3-certbot-nginx
sudo dnf install -y collectd amazon-cloudwatch-agent
sudo service docker start
sudo systemctl enable docker
## wizard config
# sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-config-wizard
## start agent
# sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -a fetch-config -m ec2 -c file:/opt/aws/amazon-cloudwatch-agent/bin/config.json -s
## status of agent
# sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -m ec2 -a status
# sudo certbot --nginx -d api.sovngarde.xyz
              EOF
}
