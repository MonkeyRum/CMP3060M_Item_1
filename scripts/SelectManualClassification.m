function [ ManualClassification ] = SelectManualClassification( I, cellWidth, cellHeight )
%UNTITLED Manually select good/bad cells within the input image

imageSize = size(I);
imageWidth = imageSize(1);
imageHeight = imageSize(2);

if(mod(imageWidth, cellWidth) ~= 0)
    warning('Image width is not equally divisible by the cell width, outlying cells will be ignored')
end
    
if(mod(imageHeight, cellHeight) ~= 0)
    warning('Image height is not equally divisible by the cell width, outlying cells will be ignored')
end

numCellsX = imageWidth / cellWidth;
numCellsY = imageHeight / cellHeight;
ManualClassification = zeros(numCellsX, numCellsY);

I(256:256:end,:,:) = 0;
I(:,256:256:end,:) = 0;

f = figure; imshow(I);
set(f,'Units', 'pixels');

while 1 == 1
    [x, y, button] = ginput(1);
    
    if(button == 1)
        xCell = uint32(floor(x / cellWidth));
        yCell = uint32(floor(y / cellHeight));
        ManualClassification(yCell + 1, xCell + 1) = 1;
        
        xPlot = xCell * cellWidth;
        yPlot = yCell * cellHeight;
        
        tl = [xPlot, yPlot];
        bl = [xPlot, yPlot + cellHeight];
        tr = [xPlot + cellWidth, yPlot];
        br = [xPlot + cellWidth, yPlot + cellHeight];
        
        p=patch([tl(1) bl(1) br(1) tr(1)],[tl(2) bl(2) br(2) tr(2)],'red');
        set(p,'FaceAlpha',0.2);
        
    else
        break
    end
end

close(f);

end