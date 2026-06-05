/**
 * Extrae un mensaje legible de un error capturado en un catch block.
 * Si el error viene de api.service (instanceof Error), usa su mensaje
 * que ya fue parseado del JSON del backend.
 * Si no, devuelve el fallback proporcionado.
 */
export function parseApiError(err: unknown, fallback = 'Ha ocurrido un error inesperado'): string {
  if (err instanceof Error && err.message) return err.message;
  if (typeof err === 'string' && err) return err;
  return fallback;
}
