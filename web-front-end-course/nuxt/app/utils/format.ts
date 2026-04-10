export const formatCurrency = (value: number | string) => {
  const num = typeof value === 'string' ? parseFloat(value) : value
  if (isNaN(num)) return '0.00'
  
  return num.toLocaleString('en-US', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  })
}

export const formatDateAfterDays = (daysToAdd: number) => {
  const date = new Date()
  date.setDate(date.getDate() + daysToAdd)
  
  return date.toLocaleDateString('en-US', {
    month: '2-digit',
    day: '2-digit',
    year: 'numeric'
  })
}