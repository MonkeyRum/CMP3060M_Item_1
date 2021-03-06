function GenerateData( filenames, outDir, w, h )
%GenerateData Generates feature data from input image.

numFiles = size(filenames);
numFiles = numFiles(2);

funs = {
            @(block_struct) var(range(double(block_struct.data))),...
            @(block_struct) skewness(range(double(block_struct.data))),...
            @(block_struct) kurtosis(range(double(block_struct.data))),...
            @(block_struct) graycoprops_fun(block_struct.data, 'Contrast'), ...
            @(block_struct) graycoprops_fun(block_struct.data, 'Correlation'), ...
            @(block_struct) graycoprops_fun(block_struct.data, 'Energy'), ...
            @(block_struct) graycoprops_fun(block_struct.data, 'Homogeneity'), ...
            @(block_struct) mean2(double(block_struct.data)), ...
            @(block_struct) median(median(double(block_struct.data))), ...
            @(block_struct) std(std(double(block_struct.data))), ...
            @(block_struct) mode(mode(double(block_struct.data))), ...
            @(block_struct) min(min(double(block_struct.data))), ...
            @(block_struct) max(max(double(block_struct.data)))
    };

for i=1:numFiles
    
    name = filenames{1, i};
    I = imread(name);
    I_HSV = uint8(rgb2hsv(I) * (255/1));
    
    images = {};
    
    IR = I(:,:,1);
    IG = I(:,:,2);
    IB = I(:,:,3);
    IGray = rgb2gray(I);
    IH = I_HSV(:,:,1);
    IS = I_HSV(:,:,2);
    IV = I_HSV(:,:,3);
    
    images = {IR IG IB IGray IH IS IV};
    tags = {'RED' 'GREEN' 'BLUE' 'GRAY' 'HUE' 'SAT' 'INT'};
    
    for j=1:length(funs)
        
        fun = funs{j};
        filename = filenames{i};
        
        for k=1:length(images)
            process_image_fun(images{k}, filename, tags{k}, fun, outDir, w, h);
        end
        
    end
    
end

end

function process_image_fun(image, imageFilename, imageName, fun, outDir, w, h)
    regex = ',';
    replace = '.';
    outFile = strcat(outDir, imageFilename, '.', imageName, func2str(fun), '.csv');
    outFile = regexprep(outFile,regex,replace);
    blocks = blockproc(image, [w, h], fun);
    csvwrite(outFile, blocks);
end

function [out_val] = graycoprops_fun (block, param)

    if(isfloat(block))
        A = []
    end
    out_val = graycoprops(block, param);
    out_val = getfield(out_val, param);
    
end