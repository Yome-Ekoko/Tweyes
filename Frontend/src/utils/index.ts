import { AxiosResponse } from "axios";
import dayjs, { Dayjs } from "dayjs";

export function returnDate(date: Dayjs) {
  let year = date.year();
  let month = date.month();
  let day = date.date();

  // Get current time
  let currentTime = dayjs();

  // Extract hours, minutes, and seconds from the current time
  let hours = currentTime.hour();
  let minutes = currentTime.minute();
  let seconds = currentTime.second();

  let newDateTimeObject = new Date(year, month, day, hours, minutes, seconds);

  // Manually format the date as "YYYY-MM-DD HH:mm:ss"
  let formattedDate =
    `${newDateTimeObject.getFullYear()}-` +
    `${String(newDateTimeObject.getMonth() + 1).padStart(2, '0')}-` +
    `${String(newDateTimeObject.getDate()).padStart(2, '0')} ` +
    `${String(newDateTimeObject.getHours()).padStart(2, '0')}:` +
    `${String(newDateTimeObject.getMinutes()).padStart(2, '0')}:` +
    `${String(newDateTimeObject.getSeconds()).padStart(2, '0')}`;

  return formattedDate;
}

export function getFileName(request: AxiosResponse<BlobPart, any>, fileExtension: string): string {
  const disposition = request.headers['content-disposition']
  let filename = `${new Date().toISOString()}.${fileExtension}`
  if (disposition && disposition.indexOf('attachment') !== -1) {
    const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/
    const matches = filenameRegex.exec(disposition)
    if (matches != null && matches[1]) {
      filename = matches[1].replace(/['"]/g, '')
    }
  }
  return filename
}
