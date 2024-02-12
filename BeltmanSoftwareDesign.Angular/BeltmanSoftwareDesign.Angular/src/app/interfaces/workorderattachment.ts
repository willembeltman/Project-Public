export interface WorkorderAttachment {
    id: number;
    fileMimeType: string | null;
    fileName: string | null;
    fileSize: number;
    fileMD5: string | null;
    fileUrl: string | null;
}