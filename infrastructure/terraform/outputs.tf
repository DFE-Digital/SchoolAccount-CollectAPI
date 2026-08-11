output "app_url" {
  value = azurerm_container_app.app.latest_revision_fqdn
}