/** Vite `public/images/brands/*.svg` with optional subpath `base`. */
export function brandImageSrc(file: 'google' | 'microsoft' | 'apple' | 'yahoo' | 'proton'): string {
  return `${import.meta.env.BASE_URL}images/brands/${file}.svg`;
}
