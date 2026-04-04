export interface Account {
    id: number | string;
    nickname: string;
    useNickname: boolean;
    firstName: string | null;
    lastName: string | null;
    fullName: string;
    language: string;
    picture: string;
    regionCode: string | null;
    countryCode: string | null;
    timezone: string;
    dateFormat: string;
    timeFormat: string;
    startOfWeek: DayOfWeek;
    createdAt: string;
    emails: Email[];
    connections: Connection[];
}

export interface Email {
    value: string;
    isPrimary: boolean;
    isConfirmed: boolean;
}

export interface Connection {
    type: ExternalService;
    identity: string;
    firstName: string | null;
    lastName: string | null;
}

export type ExternalService = 'unknown' | 'google' | 'microsoft';

export type DayOfWeek =
    | 'sunday'
    | 'monday'
    | 'tuesday'
    | 'wednesday'
    | 'thursday'
    | 'friday'
    | 'saturday';

export interface Country {
    alpha2: string;
    alpha3: string;
    region: string;
    name: string;
}

export interface Timezone {
    id: string;
    name: string;
    offset?: string;
}

export interface Confirming {
    token: string;
}

export interface UpdateRequest {
    nickname: string;
    firstName: string | null;
    lastName: string | null;
    useNickname: boolean;
    language: string;
    countryCode: string;
    timeZone: string;
    dateFormat: string;
    timeFormat: string;
    startOfWeek: DayOfWeek;
}

export interface EmailRequest {
    email: string;
}

export interface VerifyRequest {
    token: string;
    code: string;
}

export interface ApiError {
    title: string;
    type: string;
    detail: string | null;
    status: number;
    errors: Record<string, string[]>;
}

export function getCountryFlag(alpha2: string): string {
    return `https://flagcdn.com/${alpha2.toLowerCase()}.svg`;
}

export function formatTimezone(tz: Timezone): string {
    return `${tz.id} (UTC${tz.offset})`;
}
