const fs = require('fs')
const path = require('path')

// Read environment variables from process.env
const production = process.env.PRODUCTION || false
const baseUrl = process.env.BASE_URL

// Path to the environment template file
const envTemplatePath = path.join(__dirname, 'src/environments/environment.template.ts')

// Read the environment template file
let envTemplate = fs.readFileSync(envTemplatePath, 'utf8')

// Replace placeholders with actual values
envTemplate = envTemplate.replace('${PRODUCTION}', production)
envTemplate = envTemplate.replace('${BASE_URL}', baseUrl)

// Path to the environment file to be generated
const envFilePath = path.join(__dirname, 'src/environments/environment.ts')

// Write the environment file
fs.writeFileSync(envFilePath, envTemplate)

console.log(`Environment file generated at ${envFilePath}`)
