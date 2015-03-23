function [ ManualClassification ] = SelectManualClassification( imageFilename, cellWidth, cellHeight, outDir, numClasses )
%SELECTMANUALCLASSIFICATION Manually select good/bad cells within the input image
%   imageFile   - file name of image
%   cellWidth   - width of patches
%   cellHeight  - height of patches
%   outDir      - manual classification result output directory

I = imread(imageFilename);
imageSize = size(I);
imageWidth = imageSize(1);
imageHeight = imageSize(2);

% Ignoring outlying cells is not implemented but might be in the future
if(mod(imageWidth, cellWidth) ~= 0)
    warning('Image width is not equally divisible by the cell width, outlying cells will be ignored')
end
    
if(mod(imageHeight, cellHeight) ~= 0)
    warning('Image height is not equally divisible by the cell width, outlying cells will be ignored')
end

colours = {'red', 'green', 'blue'};

numCellsX = imageWidth / cellWidth;
numCellsY = imageHeight / cellHeight;
ManualClassification = zeros(numCellsX, numCellsY);
PatchHandles = zeros(numCellsX, numCellsY);

I(256:256:end,:,:) = 0;
I(:,256:256:end,:) = 0;

f = figure; imshow(I);
set(f, 'Units', 'pixels');

while 1 == 1
    [x, y, button] = ginput(1);
    
    if(button == 1)
        xCell = uint32(floor(x / cellWidth));
        yCell = uint32(floor(y / cellHeight));
        
        ManualClassification(yCell + 1, xCell + 1) = ManualClassification(yCell + 1, xCell + 1) + 1;
        if(ManualClassification(yCell + 1, xCell + 1) > numClasses)
            ManualClassification(yCell + 1, xCell + 1) = 0;
        end
            
        if(ManualClassification(yCell + 1, xCell + 1) > 0)
            %ManualClassification(yCell + 1, xCell + 1) = 1;

            xPlot = xCell * cellWidth;
            yPlot = yCell * cellHeight;

            tl = [xPlot, yPlot];
            bl = [xPlot, yPlot + cellHeight];
            tr = [xPlot + cellWidth, yPlot];
            br = [xPlot + cellWidth, yPlot + cellHeight];

            if(PatchHandles(yCell + 1, xCell + 1) ~= 0)
                delete(PatchHandles(yCell + 1, xCell + 1));
                PatchHandles(yCell + 1, xCell + 1) = 0;
            end
            colour = colours{ManualClassification(yCell + 1, xCell + 1)};
            PatchHandles(yCell + 1, xCell + 1) = patch([tl(1) bl(1) br(1) tr(1)],[tl(2) bl(2) br(2) tr(2)],colour, 'FaceAlpha', 0.2);
            set(PatchHandles(yCell + 1, xCell + 1));
        else
            ManualClassification(yCell + 1, xCell + 1) = 0;
            delete(PatchHandles(yCell + 1, xCell + 1));
            PatchHandles(yCell + 1, xCell + 1) = 0;
        end
        
    else
        break
    end
end

close(f);

[~, name, ~] = fileparts(imageFilename);
outFile = strcat(outDir, name, '_manualClassification.csv');
csvwrite(outFile, ManualClassification);
disp(strcat('Wrote manual classification file: ', outFile));

end