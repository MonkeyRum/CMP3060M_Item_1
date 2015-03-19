function OutputPredictions( ClassLabelPredication, length, width )
%UNTITLED3 Summary of this function goes here
%   length - number of labels per file
%   width - width of row in each file

M = [1 2 3; 4 5 6; 7 8 9];
V = M(:);
M2 = vec2mat(V,3);

T = [];
height = length / width;

num_labels = size(ClassLabelPredication);
num_labels = num_labels(1);

for k=1:num_labels
   
    start = ((k-1)*length) + 1;
    T = (start:start+length-1);
    T = reshape(T, [height, width]);
    
end

end

