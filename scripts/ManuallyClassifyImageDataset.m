function ManuallyClassifyImageDataset( imageDir, outDir, cellWidth, cellHeight )
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

filelist = dir([fileparts(imageDir) filesep '*.JPG']);
fileNames = {filelist.name};

numImageFiles = size(fileNames, 2);

for i = 1 : numImageFiles
    SelectManualClassification(fileNames{i}, cellWidth, cellHeight, outDir);
end

end

