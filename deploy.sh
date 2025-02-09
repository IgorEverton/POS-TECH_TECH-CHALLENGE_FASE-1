# 1. Construção das imagens
echo "🔨 Construindo imagens Docker..."
docker compose build

# 2. Aplicação dos manifests no Kubernetes
echo "🔄 Convertendo docker-compose.yml para manifests Kubernetes..."
/mnt/c/Windows/System32/kompose.exe convert -f docker-compose.yml -o .

echo "📜 Aplicando recursos do Kubernetes..."
kubectl apply -f .

echo "✅ Deploy concluído!"