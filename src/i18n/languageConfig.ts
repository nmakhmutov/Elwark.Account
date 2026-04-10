type LanguageConfig = {
  label: string;
  locales: readonly string[];
};

type LanguageConfigMap = Record<string, LanguageConfig>;

const LANGUAGE_CONFIG = {
  en: {
    label: 'English',
    locales: ['en-US', 'en-GB', 'en-CA', 'en-AU', 'en-NZ', 'en-IE'],
  },
  ru: {
    label: 'Русский',
    locales: ['ru-RU', 'ru-BY', 'ru-KZ'],
  },
} as const satisfies LanguageConfigMap;

export type SupportedLanguage = keyof typeof LANGUAGE_CONFIG;

export interface LanguageOption {
  value: SupportedLanguage;
  label: string;
}

export interface LocaleOption {
  value: string;
  label: string;
}

export const LANGUAGE_OPTIONS: LanguageOption[] = Object.entries(LANGUAGE_CONFIG).map(
  ([value, config]) => ({
    value: value as SupportedLanguage,
    label: config.label,
  })
);

const CONFIGURED_LOCALES = Array.from(
  new Set(Object.values(LANGUAGE_CONFIG).flatMap((config) => config.locales))
);

export function resolveSupportedLanguage(value: string): SupportedLanguage {
  const base = value.split('-')[0] as SupportedLanguage;
  return base in LANGUAGE_CONFIG ? base : 'en';
}

function formatLocaleLabel(locale: string, displayLanguage: string): string {
  try {
    const parsed = new Intl.Locale(locale);
    const languageNames = new Intl.DisplayNames([displayLanguage], { type: 'language' });
    const regionNames = new Intl.DisplayNames([displayLanguage], { type: 'region' });
    const languageLabel = languageNames.of(parsed.language);
    const regionLabel = parsed.region ? regionNames.of(parsed.region) : undefined;

    if (languageLabel && regionLabel) {
      return `${languageLabel} (${regionLabel})`;
    }

    if (languageLabel) {
      return languageLabel;
    }
  } catch {
    // Fall back to the raw locale code when Intl helpers cannot parse it.
  }

  return locale;
}

function prependCurrentLocale(locales: readonly string[], currentLocale?: string): string[] {
  if (!currentLocale || locales.includes(currentLocale)) {
    return [...locales];
  }

  return [currentLocale, ...locales];
}

export function getLocaleOptions(params: {
  currentLocale?: string;
  displayLanguage: string;
}): LocaleOption[] {
  const locales = prependCurrentLocale(CONFIGURED_LOCALES, params.currentLocale);

  return locales.map((locale) => ({
    value: locale,
    label: formatLocaleLabel(locale, params.displayLanguage),
  }));
}
