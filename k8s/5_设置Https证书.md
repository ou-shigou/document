在 Azure 上部署并配置 Kubernetes 服务（AKS）以支持 HTTPS 连接时，您可以利用 Azure 提供的一些特定功能和资源。这包括使用 Azure 的原生工具和服务，如 Azure Key Vault 来存储密钥和证书，以及 Azure 提供的托管的 Kubernetes 服务（AKS）。以下是在 Azure 上设置 HTTPS 的基本步骤：

### 1. 配置 Azure Kubernetes Service (AKS)
如果您还没有 AKS 实例，您需要创建一个。可以通过 Azure 门户、Azure CLI 或 Azure Resource Manager 模板来创建 AKS。

### 2. 获取 SSL/TLS 证书
您可以通过 Azure Key Vault 获取和管理 SSL/TLS 证书，或者使用 Let's Encrypt 等免费证书服务。Azure Key Vault 提供了一个安全的位置来管理您的证书和密钥。

### 3. 创建 Kubernetes Secret
将您的 SSL/TLS 证书和私钥存储在 Kubernetes Secret 中。这可以通过 kubectl 命令行工具完成。例如：

```bash
kubectl create secret tls my-tls-secret --cert=path/to/cert.crt --key=path/to/cert.key
```

### 4. 配置 Ingress 资源
在 AKS 集群中配置 Ingress 资源来使用您的证书。这通常涉及创建一个 Ingress YAML 配置，指定您的服务和 TLS Secret。

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: my-ingress
spec:
  tls:
  - hosts:
    - example.com
    secretName: my-tls-secret
  rules:
  - host: example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: my-service
            port:
              number: 80
```

### 5. 使用 Azure 提供的 Ingress 控制器
Azure 提供了自己的 Ingress 控制器，例如 Application Gateway Ingress Controller (AGIC)。AGIC 集成了 Azure Application Gateway，为您的 Kubernetes 应用提供了自动的 HTTPS 支持和高级路由功能。

### 6. 更新 DNS 设置
将您的域名 DNS 记录指向 AKS Ingress 控制器的外部 IP 地址。

### 7. 监控和日志
利用 Azure Monitor 和 Azure Log Analytics 来监控您的 AKS 环境和应用性能。

### 注意
- 确保您的 AKS 集群有适当的网络配置，以便外部流量可以到达 Ingress 控制器。
- 如果您使用的是 Azure 提供的托管证书服务，确保您的服务和 Ingress 配置与之兼容。

通过这些步骤，您可以在 Azure Kubernetes Service (AKS) 上成功地为您的服务设置 HTTPS 支持，同时利用 Azure 提供的高级功能和服务来增强安全性和性能。