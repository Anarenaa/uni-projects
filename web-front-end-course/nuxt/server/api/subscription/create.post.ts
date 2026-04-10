export default defineEventHandler(async (event) => {
  const body = await readBody(event);

  const { card, exp, cvc, name, address, terms } = body

  if (!card || !exp || !cvc || !name || !address || !terms) {
    throw createError({
      statusCode: 400,
      statusMessage: 'All fields are required and terms must be accepted'
    })
  }

  const cardRegex = /^\d{16}$/;
  const expRegex = /^(0[1-9]|1[0-2])\/[0-9]{2}$/;
  const cvcRegex = /^\d{3,4}$/;

  if (!cardRegex.test(card.replace(/\s/g, '')) || 
      !expRegex.test(exp) || 
      !cvcRegex.test(cvc)) {
    throw createError({
      statusCode: 400,
      statusMessage: 'Invalid payment details format',
    });
  }

  const [m, y] = exp.split('/').map(Number)
  const now = new Date()
  const currentYear = now.getFullYear() % 100
  const currentMonth = now.getMonth() + 1

  if (y < currentYear || (y === currentYear && m < currentMonth)) {
    throw createError({
      statusCode: 400,
      statusMessage: 'The card has expired'
    })
  }
  
  return {
    success: true,
    message: 'Subscription created successfully',
    subscriptionId: Math.random().toString(36).substr(2, 9)
  }
})