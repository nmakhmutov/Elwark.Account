import type { DayOfWeek } from './api/types';

export const DATE_FORMATS = [
  'dd.MM.yyyy',
  'dd.MM.yy',
  'd.M.yyyy',
  'd.M.yy',
  'dd-MM-yyyy',
  'dd-MM-yy',
  'd-M-yyyy',
  'd-M-yy',
  'dd/MM/yyyy',
  'dd/MM/yy',
  'd/M/yyyy',
  'd/M/yy',
  'yyyy-MM-dd',
  'MM.dd.yyyy',
  'MM-dd-yyyy',
  'MM/dd/yyyy',
] as const;

export const TIME_FORMATS = [
  'H:mm',
  'HH:mm',
  'HH:mm:ss',
  'h:mm tt',
  'hh:mm tt',
] as const;

export const DAYS_OF_WEEK: DayOfWeek[] = [
  'monday',
  'tuesday',
  'wednesday',
  'thursday',
  'friday',
  'saturday',
  'sunday',
];

export const DAY_OF_WEEK_LABELS: Record<DayOfWeek, string> = {
  sunday: 'Sunday',
  monday: 'Monday',
  tuesday: 'Tuesday',
  wednesday: 'Wednesday',
  thursday: 'Thursday',
  friday: 'Friday',
  saturday: 'Saturday',
};

export function formatDateExample(format: string): string {
  const now = new Date();
  const d = now.getDate();
  const dd = String(d).padStart(2, '0');
  const M = now.getMonth() + 1;
  const MM = String(M).padStart(2, '0');
  const yyyy = String(now.getFullYear());
  const yy = yyyy.slice(2);
  const H = now.getHours();
  const HH = String(H).padStart(2, '0');
  const h = H % 12 || 12;
  const hh = String(h).padStart(2, '0');
  const mm = String(now.getMinutes()).padStart(2, '0');
  const ss = String(now.getSeconds()).padStart(2, '0');
  const tt = H >= 12 ? 'PM' : 'AM';

  return format
    .replace('dd', dd)
    .replace('d', String(d))
    .replace('MM', MM)
    .replace('M', String(M))
    .replace('yyyy', yyyy)
    .replace('yy', yy)
    .replace('HH', HH)
    .replace('H', String(H))
    .replace('hh', hh)
    .replace('h', String(h))
    .replace('mm', mm)
    .replace('ss', ss)
    .replace('tt', tt);
}
